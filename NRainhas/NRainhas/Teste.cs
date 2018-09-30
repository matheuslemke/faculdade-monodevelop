using System;

namespace NRainhas
{
    public class Teste
    {
        private string nome;

        public string Nome
        {
            get
            {
                return this.nome;
            }
            set
            {
                nome = value;
            }
        }

        public Teste(string nome)
        {
            this.nome = nome;
        }

        public override string ToString()
        {
            return string.Format("[Teste: nome={0}]", nome);
        }
    }
}

