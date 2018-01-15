using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    public enum BraidLinkDiscriminationResult
    {
        NoOpinion = 0,
        No = -1,
        Yes = 1,
        HardNo = -1000,
        ISaidYes = 100000
    }
}
