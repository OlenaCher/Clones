using System;
using System.Collections.Generic;

namespace Clones
{
	public class CloneVersionSystem : ICloneVersionSystem
	{
        private List<Clone> clonesList = new List<Clone>();
        public string Execute(string query)
		{
            if (clonesList.Count == 0) clonesList.Add(new Clone());
            var commandArr = query.Split(' ');
            var command = commandArr[0];
            var cloneIndex =Convert.ToInt32(commandArr[1])-1;
            switch (command)
            {
                case "learn":
                    clonesList[cloneIndex].Learn(commandArr[2]);
                    break;
                case "rollback":
                    clonesList[cloneIndex].Rollback();
                    break;
                case "relearn":
                    clonesList[cloneIndex].Relearn();
                    break;
                case "clone":
                    clonesList.Add(new Clone(clonesList[cloneIndex]));
                    break;
                case "check":
                    return clonesList[cloneIndex].Check();
            }
			return null;
		}
        
        public class Clone
        {
            public ItemsStack<string> Programs;
            public ItemsStack<string> Cancells;

            public Clone()
            {
                Programs = new ItemsStack<string>();
                Cancells = new ItemsStack<string>();
            }
 
            public Clone(Clone clonedItem)
            {
                Programs = new ItemsStack<string>()
                    { Head = clonedItem.Programs.Head , Tail = clonedItem.Programs.Tail };
                Cancells = new ItemsStack<string>()
                    { Head = clonedItem.Cancells.Head, Tail = clonedItem.Cancells.Tail };
            }

            public void Learn(string program)
            {
                Programs.Push(program);
                Cancells=new ItemsStack<string>();
            }

            public void Rollback()
            {
                Cancells.Push(Programs.Pop());
            }

            public void Relearn()
            {
                Programs.Push(Cancells.Pop());
            }

            public string Check()
            {
                if (Programs.Head==null)
                    return "basic";
                else
                {
                    var temp = Programs.Pop();
                    Programs.Push(temp);
                    return temp;
                }
            }
        }
   	}

    public class Items<T>
    {
        public T Value { get; set; }
        public Items<T> Next { get; set; }
        public Items<T> Prew { get; set; }
    }

    public class ItemsStack<T>
    {
        public Items<T> Head;
        public Items<T> Tail;


        public void Push(T item)
        {
            if (Head == null)
                Tail = Head = new Items<T> { Value = item, Next = null, Prew = null };
            else
            {
                var newItem = new Items<T> { Value = item, Next = null, Prew = Tail };
                Tail.Next = newItem;
                Tail = newItem;
            }
        }

        public T Pop()
        {
            if (Tail == null) return default(T);
            var result = Tail.Value;
            Tail = Tail.Prew;
            if (Tail == null)
                Head = null;
            else
                Tail.Next = null;
            return result;
        }
    }
}



