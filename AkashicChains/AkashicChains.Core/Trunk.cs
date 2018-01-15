using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace AkashicChains.Core
{
    public class Trunk
    {
        readonly ISubject<ChainLink> _braidBuilderSubject = new Subject<ChainLink>();

        public void Accept(MarkovEvent markovEvent)
        {
            // build a link

            var chainLink = ChainLink.Build(markovEvent);

            _braidBuilderSubject.OnNext(chainLink);
        }

        public void AddBraid(BraidBuilder braidBuilder)
        {
            // build and assign Braid

            var braid = braidBuilder.BuildBraid(this);

            _braidBuilderSubject.Subscribe(braid.Accept);
        }
    }
}
