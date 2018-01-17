using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace AkashicChains.Core
{
    public class Braid : IDisposable
    {
        readonly ISubject<ChainLink> _chainBuilderSubject = new Subject<ChainLink>();

        private readonly int _addThreshold;

        private readonly BraidLinkDiscriminators _discriminators;
        private readonly ChainBuilder _chainBuilder;
        private IDisposable _subscription;
        private readonly Dictionary<ChainIdentity, Chain> _chains = new Dictionary<ChainIdentity, Chain>();


        private Braid(BraidLinkDiscriminators discriminators, ChainBuilder chainBuilder, int addThreshold)
        {
            _discriminators = discriminators;
            _chainBuilder = chainBuilder;
            _addThreshold = addThreshold;
        }

        internal static Braid Build(BraidLinkDiscriminators discriminators, ChainBuilder chainBuilder, int addThreshold = 1)
        {
            var braid = new Braid(discriminators, chainBuilder, addThreshold);

            chainBuilder.AddBraid(braid);

            chainBuilder.AcceptAddChainAction(braid._chains.ContainsKey, braid._chains.Add, x => braid._chains[x]);

            // wire up async here
            var subscription = braid._chainBuilderSubject.Subscribe(x =>
            {
                var discriminationResult = braid._discriminators.Discriminate(x);

                if (discriminationResult >= braid._addThreshold)
                {
                    braid._chainBuilder.Accept(x);
                }
            });

            braid._subscription = subscription;

            return braid;
        }

        internal void Accept(ChainLink chainLink)
        {
            // go async
            _chainBuilderSubject.OnNext(chainLink);
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}
