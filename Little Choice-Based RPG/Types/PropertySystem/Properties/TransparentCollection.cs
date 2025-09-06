using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Properties
{
    internal class TransparentCollection<T> : ITransparent, ITransparentCollection<T> where T : notnull, ITransparentCollectionSource<T>
    {
        public void Add(T t) => t.Add()
    }
}
