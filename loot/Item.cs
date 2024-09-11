using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loot
{
    public class Item
    {
        public Item()
        {
            Value = 0;
            Name = "";
            ID = Guid.NewGuid();
        }

        public void Use()
        {
            OnUse();
        }

        protected virtual void OnUse()
        {
        }

        public Guid ID { get; }
        public int Value { get; set; }
        public string Name { get; set; }
    }
}
