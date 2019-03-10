using System;
using System.Collections.Generic;

namespace Clones
{
	public class CloneVersionSystem : ICloneVersionSystem
	{
        private List<Clone> ClonesList = new List<Clone>();
        public string Execute(string query)
		{
            if (ClonesList.Count == 0) ClonesList.Add(new Clone());
            var commandArr = query.Split(' ');
            var command = commandArr[0];
            var cloneIndex =Convert.ToInt32(commandArr[1])-1;
            switch (command)
            {
                case "learn":
                    ClonesList[cloneIndex].Learning(commandArr[2]);
                    break;
                case "rollback":
                    ClonesList[cloneIndex].Rollback();
                    break;
                case "relearn":
                    ClonesList[cloneIndex].Relearn();
                    break;
                case "clone":
                    ClonesList.Add(new Clone(ClonesList[cloneIndex]));
                    return null;
                case "check":
                    var program = ClonesList[cloneIndex].Checking();
                    return program;
            }
			return null;
		}
        
        public class Clone
        {
            public ItemsStack<string> programs;
            public ItemsStack<string> cancells;

            public Clone()
            {
                programs = new ItemsStack<string>();
                cancells = new ItemsStack<string>();
            }

            public Clone(Clone clonedItem)
            {
                programs = new ItemsStack<string>();
                var item = clonedItem.programs.head;
                for (var i = 0; i < clonedItem.programs.Count; i++)
                {
                    programs.Push(item.Value);
                    item = item.Next;
                }
                cancells = new ItemsStack<string>();
                item = clonedItem.cancells.head;
                for (var i = 0; i < clonedItem.cancells.Count; i++)
                {
                    cancells.Push(item.Value);
                    item = item.Next;
                }
            }

                public void Learning(string program)
            {
                programs.Push(program);
                while(cancells.Count!=0)
                cancells.Pop();
            }

            public void Rollback()
            {
                cancells.Push(programs.Pop());
            }

            public void Relearn()
            {
                programs.Push(cancells.Pop());
            }

            public string Checking()
            {
                if (programs.Count == 0)
                    return "basic";
                else
                    { var temp = programs.Pop();
                    programs.Push(temp);
                    return temp;
                }

            }
        }
   	}

    /*
    public class ItemsStack<T> : LinkedList<T>
    {
        public void Push(T item)
        {
            AddLast(item);
        }
       
        public T Pop()
        {
            var result = Last.Value;
            RemoveLast();
            return result;
        }
    }
    */

    public class Items<T>
    {
        public T Value { get; set; }
        public Items<T> Next { get; set; }
        public Items<T> Prew { get; set; }
    }

    public class ItemsStack<T>
    {
        public Items<T> head;
        public Items<T> tail;
        int counter;

        public void Push(T item)
        {
            if (head == null)
                tail = head = new Items<T> { Value = item, Next = null, Prew = null };
            else
            {
                var newItem = new Items<T> { Value = item, Next = null, Prew = tail };
                tail.Next = newItem;
                tail = newItem;
            }
            counter++;
        }

        private void DeleteExtraItem()
        {
            head = head.Next;
            head.Prew = null;
            counter--;
        }

        public T Pop()
        {
            if (tail == null) return default(T);
            var result = tail.Value;
            tail = tail.Prew;
            if (tail == null)
                head = null;
            else
                tail.Next = null;
            counter--;
            return result;
        }

        public int Count
        {
            get
            {
                return counter;
            }
        }
    }
}



