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
                    break;
                case "check":
                    return ClonesList[cloneIndex].Checking();
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
                programs = new ItemsStack<string>() { head = clonedItem.programs.head , tail = clonedItem.programs.tail };
                cancells = new ItemsStack<string>() { head = clonedItem.cancells.head, tail = clonedItem.cancells.tail };
            }
            public void Learning(string program)
            {
                programs.Push(program);
                cancells=new ItemsStack<string>();
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
                if (programs.head==null)
                    return "basic";
                else
                    {
                    var temp = programs.Pop();
                    programs.Push(temp);
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
        public Items<T> head;
        public Items<T> tail;


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
        }

        private void DeleteExtraItem()
        {
            head = head.Next;
            head.Prew = null;
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
            return result;
        }
    }
}



