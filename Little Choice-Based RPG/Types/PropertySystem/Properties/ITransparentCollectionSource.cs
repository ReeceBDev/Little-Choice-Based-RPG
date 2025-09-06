using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Properties
{
    internal interface ITransparentCollectionSource<T>
    {
        public ImmutableArray<T> GetAll();
        public void Add(T t);
        public void Remove(T t);
    }
}
