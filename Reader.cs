﻿namespace Library
{
    internal class Reader
    {
        private string name;

        public Reader(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }
    }
}
