//HintName: TransactionSet.Implementation.g.cs
using System.Collections.Generic;
using EdiSource.Domain.Identifiers;

namespace EdiSource.Loops
{
    public partial class TransactionSet
    {
        public List<IEdi?> EdiItems => new List<IEdi?> { ST, DTP, REFs, Loop2000, Loop2100s, SE };
    }
}
