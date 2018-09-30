using System;
using System.Collections;

namespace NRainhas
{
    public class Tabuleiro
    {
        private int[] config;

        public int[] Config
        {
            get
            {
                return this.config;
            }
        }

        private int n;

        public int N
        {
            get{ return this.n;}
        }

        public Tabuleiro()
        {
            config = new int[n];
        }

        public Tabuleiro(int n)
        {
            this.n = n;
            config = new int[n];
        }

        public void addRainha(int rainha, int i)
        {
            config[i] = rainha;
        }

        public override string ToString()
        {
            string str = "";
            foreach (int rainha in config)
            {
                str += rainha + " ";
            }
            str += "\n";
            return str;
        }
    }
}

