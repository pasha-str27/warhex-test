//завдання 4 
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Fugas_C_sharp
{

    class Message
    {
        int index;
        int fatherIndex;
        string text;
        string author;
        DateTime time;

        public Message(int index, int fatherIndex, string text, string author, DateTime time)
        {
            this.index = index;
            this.fatherIndex = fatherIndex;
            this.text = text;
            this.author = author;
            this.time = time;
        }

        public int Index
        {
            get
            {
                return index;
            }
        }

        public int FatherIndex
        {
            get
            {
                return fatherIndex;
            }
        }

        public DateTime Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }

        public string Author
        {
            get
            {
                return author;
            }
        }

        public override string ToString()
        {
            return "index: " + index + "\nfather index: " + fatherIndex + "\nauthor: " + author + "\ttime: " + time.ToString() + "\n" + text + "\n";
        }
    }

    class Chat
    {
        List<Message> messages;

        public Chat()
        {
            messages = new List<Message>();
        }

        public void addMessage(Message newMessage)
        {
            messages.Add(newMessage);
        }

        //сортування за датою(від старіших до новіших повідомлень)
        public void sortByTime()
        {
            for (int i = 0; i < messages.Count; ++i)
                for (int j = i + 1; j < messages.Count; ++j)
                    if (messages[i].Time > messages[j].Time)
                    {
                        DateTime tmp = messages[i].Time;
                        messages[i].Time = messages[j].Time;
                        messages[j].Time = tmp;
                    }
        }

        //пошук за атором
        public void findMessagesByAuthor(string author)
        {
            foreach (Message mes in messages)
                if (mes.Author == author)
                    Console.WriteLine(mes.ToString());
        }

        //пошук відповіді на повідомлення
        public void printAnswersOnMessage(Message needMessage)
        {
            foreach (Message mes in messages)
                if (mes.FatherIndex == needMessage.Index)
                    Console.WriteLine(mes.ToString());
        }

        public override string ToString()
        {
            string info = "";
            foreach (Message mes in messages)
                info += mes.ToString() + "\n";
            return info;
        }

        ~Chat()
        {
            messages.Clear();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Chat chat = new Chat();
            chat.addMessage(new Message(1, 0, "jsjsjsjs xkxkxl kx", "anonim", new DateTime(2021, 2, 27, 20, 23, 23)));
            chat.addMessage(new Message(2, 1, "jsjc  cxl k", "anonim2", new DateTime(2021, 2, 27, 21, 20, 22)));
            chat.addMessage(new Message(3, 1, "js", "anonim3", new DateTime(2021, 2, 27, 20, 20, 20)));
            chat.addMessage(new Message(4, 3, "js and html", "anonim4", new DateTime(2021, 2, 27, 21, 21, 21)));
            chat.addMessage(new Message(5, 1, "lol", "anonim", new DateTime(2021, 2, 27, 19, 23, 23)));
            chat.addMessage(new Message(6, 3, "text", "anonim2", new DateTime(2021, 2, 21, 23, 23, 23)));
            Console.WriteLine("given chat: \n" + chat.ToString());
            chat.sortByTime();
            Console.WriteLine("chat after sorting: \n" + chat.ToString());
            Console.Write("enter index to find answers on message with this index: ");
            int index = int.Parse(Console.ReadLine());
            chat.printAnswersOnMessage(new Message(index, -1, "", "", new DateTime()));
            Console.Write("enter author of messages to find messages: ");
            string author = Console.ReadLine();
            chat.findMessagesByAuthor(author);
        }
    }
}