using System;
using System.Collections;

namespace NRainhas
{
    public class Solucionador
    {
        private int nGeracoes, nPopulacao, n, fitnessCorte;
        private int nCruzados, nMutantes;
        private int[,] geracao;
        private Random random = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));

        public Solucionador()
        {
        }

        public Solucionador(int taxaCr, int taxaMt, int nGeracoes, int nPopulacao, int n, int fitnessCorte)
        {
            this.nGeracoes = nGeracoes;
            this.nPopulacao = nPopulacao;
            this.n = n;
            geracao = new int[nPopulacao, n + 1];
            nCruzados = (nPopulacao * taxaCr) / 100;
            nMutantes = (nPopulacao * taxaMt) / 100;
            this.fitnessCorte = fitnessCorte;
        }

        /// <summary>
        /// Inicia o processamento do algoritmo.
        /// Partindo de uma geração aleatória, são geradas nGeracoes
        /// e sobre essas, são aplicadas o cruzamento, mutação e a seleção.
        /// </summary>
        /// <returns>Uma lista com a última geracao.</returns>
        public int[,] iniciar()
        {

            calcularGeracaoAleatoria();

            calcularFitnessDaGeracao();

            for (int i = 0; i < nGeracoes-1; i++)
            {
                //if (i % 100 == 0)
                //    mostrarGeracao();
                //Console.WriteLine("\n\nGeração número " + i);

                //mostrarMelhor();

                //selecionarMenoresFitness();
                cruzarMetadeMetade();

                mutar();

                calcularFitnessDaGeracao();

                //selecionarMaioresFitness();
                selecionarMenoresFitness();

                calcularFitnessDaGeracao();
            }

            //int[] ind = {3, 7, 0, 4, 6, 1, 5, 2};
            //addIndividuo(ind, n-1);
            //calcularFitnessDaGeracao();

            //Console.WriteLine("Fitness: " + geracao[n-1, n]);

            //rankearGeracaoPartindoDoMenor();
            rankearGeracaoPartindoDoMaior();

            return geracao;
        }

        private void mostrarMenor()
        {
            int menor = Int16.MaxValue;
            for (int i = 0; i < nPopulacao; i++)
            {
                if (geracao[i, n] < menor)
                    menor = geracao[i, n];
            }

            Console.WriteLine("Melhor: " + menor);
        }

        private void addIndividuo(int[] ind, int i)
        {
            for (int j = 0; j < n; j++)
            {
                geracao[i, j] = ind[j];
            }
        }

        /// <summary>
        /// Calcula a primeira geracao que é aleatoria.
        /// </summary>
        /// <returns>A geracao aleatoria.</returns>
        private void calcularGeracaoAleatoria()
        {
            for (int i = 0; i < nPopulacao; i++)
            {
                calcularIndividuoAleatorio(i);
            }
        }

        private void calcularIndividuoAleatorio(int i)
        {
            for (int j = 0; j < n; j++)
            {
                int aleatorio = random.Next(0, n);
                //Console.WriteLine("[" + i + ", " + j + "] = " + ale);
                geracao[i, j] = aleatorio;
            }
        }

        private void calcularFitnessDaGeracao()
        {

            for (int i = 0; i < nPopulacao; i++)
            {
                calcularFitnessDoIndivividuo(i);
                //calcularFuncaoAvaliacaoDoIndividuo(i);
            }
        }

        private void calcularFuncaoAvaliacaoDoIndividuo(int i)
        {
            int diferencaLinhas, diferencaValores, nNaoAtacadas = 0;
            geracao[i, n] = 0; //inicializa o valor de fitness

            for (int j = 0; j < n-1; j++)
            {
                for (int k = j+1; k < n; k++)
                    //for (int k = 0; k < n; k++)
                {
                    if (geracao[i, j] != geracao[i, k])
                    {
                        diferencaLinhas = Math.Abs(j - k);
                        diferencaValores = Math.Abs(geracao[i, j] - geracao[i, k]);
                        if (diferencaLinhas == diferencaValores)
                        {
                            nNaoAtacadas++;
                        }
                    }
                }
            }

            geracao[i, n] = (nNaoAtacadas * (nNaoAtacadas - 1)) / 2;
        }

        private void calcularFitnessDoIndivividuo(int i)
        {
            int diferencaLinhas, diferencaValores;
            geracao[i, n] = 0; //inicializa o valor de fitness
            for (int j = 0; j < n-1; j++)
            {
                for (int k = j+1; k < n; k++)
                //for (int k = 0; k < n; k++)
                {
                    if (geracao[i, j] == geracao[i, k])
                    {
                        geracao[i, n]++;
                    }

                    diferencaLinhas = Math.Abs(j - k);
                    diferencaValores = Math.Abs(geracao[i, j] - geracao[i, k]);
                    if (diferencaLinhas == diferencaValores)
                    {
                        geracao[i, n]++;
                    }
                }
            }
        }

        private void selecionarMaioresFitness()
        {
            for (int i = 0; i < nPopulacao; i++)
            {
                if (geracao[i, n] < fitnessCorte)
                    calcularIndividuoAleatorio(i);
            }
        }

        private void selecionarMenoresFitness()
        {
            for (int i = 0; i < nPopulacao; i++)
            {
                if (geracao[i, n] > fitnessCorte)
                    calcularIndividuoAleatorio(i);
            }
        }

        private void rankearGeracaoPartindoDoMenor()
        {
            int indice;

            for (int i = 0; i < nPopulacao-1; i++)
            {
                indice = menorFitnessPartindoDeX(i);
                trocarLinhas(indice, i);
            }
        }

        private void rankearGeracaoPartindoDoMaior()
        {
            int indice;

            for (int i = 0; i < nPopulacao-1; i++)
            {
                indice = maiorFitnessPartindoDeX(i);
                trocarLinhas(indice, i);
            }
        }

        private int menorFitnessPartindoDeX(int x)
        {
            int menorFitness = System.Int16.MaxValue, indice = -1;
            for (int i = x; i < nPopulacao; i++)
            {
                if (geracao[i, n] < menorFitness)
                {
                    menorFitness = geracao[i, n];
                    indice = i;
                }
            }
            return indice;
        }

        private int maiorFitnessPartindoDeX(int x)
        {
            int maiorFitness = System.Int16.MinValue, indice = -1;
            for (int i = x; i < nPopulacao; i++)
            {
                if (geracao[i, n] > maiorFitness)
                {
                    maiorFitness = geracao[i, n];
                    indice = i;
                }
            }
            return indice;
        }

        private void trocarLinhas(int linhaSource, int linhaDest)
        {
            int[] aux = new int[n + 1];

            for (int i = 0; i < n+1; i++)
            {
                aux[i] = geracao[linhaDest, i];
            }

            for (int i = 0; i < n+1; i++)
            {
                geracao[linhaDest, i] = geracao[linhaSource, i];
            }

            for (int i = 0; i < n+1; i++)
            {
                geracao[linhaSource, i] = aux[i];
            }
        }

        /// <summary>
        /// Cruza uma geracao específica.
        /// Se nCruzados é ímpar, o número de indivíduos cruzados
        /// é incrementado internamente -- i+=2 -- , a fim de virar par.
        /// 
        /// <param name="geracao">Geracao a ser cruzada.</param>
        /// <returns>A geração cruzada.</returns>
        private void cruzarMetadeMetade()
        {
            int individuoUm, individuoDois, individuoSalvar, alelosCruzar = n / 2;

            //Console.WriteLine(alelosCruzar);

            for (int i = 0; i < nCruzados; i+=2)
            {
                individuoUm = random.Next(0, nPopulacao);
                individuoDois = random.Next(0, nPopulacao);

                while (individuoUm == individuoDois)
                {
                    individuoDois = random.Next(0, nPopulacao);
                }

                if (geracao[individuoUm, n] > geracao[individuoDois, n])
                    individuoSalvar = individuoUm;
                else
                    individuoSalvar = individuoDois;

                if (individuoSalvar == individuoUm)
                {
                    for (int j = alelosCruzar; j < n; j++)
                    {
                        geracao[individuoSalvar, j] = geracao[individuoDois, j];
                    }
                }
                else
                {
                    for (int j = alelosCruzar; j < n; j++)
                    {
                        //Console.WriteLine("j: " + j);
                        geracao[individuoSalvar, j] = geracao[individuoUm, j];
                    }
                }
            }
        }

        private void mutar()
        {
            for (int i = 0; i < nMutantes; i++)
            {
                int individuoMutante = random.Next(0, nPopulacao);
                int colunaParaMutar = random.Next(0, n);
                int novoValor = random.Next(0, n);

                //              Console.WriteLine("Mutando a coluna " + colunaParaMutar + " do individuo " + individuoMutante + " para " + novoValor);

                geracao[individuoMutante, colunaParaMutar] = novoValor;
            }
        }

        public void mostrarGeracao()
        {
            Console.Write("       ");
            for (int j = 0; j < n; j++)
            {
                Console.Write("  " + j);
            }
            Console.WriteLine("     F");

            Console.Write("       ");
            for (int j = 0; j < n; j++)
            {
                Console.Write("___");
            }
            Console.WriteLine("__");

            for (int i = 0; i < nPopulacao; i++)
            {
                if (i < 10)
                    Console.Write(i + "     |");
                else if (i < 100)
                    Console.Write(i + "    |");
                else if (i < 1000)
                    Console.Write(i + "   |");
                else if (i < 10000)
                    Console.Write(i + "  |");
                else if (i < 100000)
                    Console.Write(i + " |");
                else if (i < 1000000)
                    Console.Write(i + "|");

                for (int j = 0; j < n+1; j++)
                {
                    if (j == n)
                        Console.Write("  |  " + geracao[i, j]);
                    else
                        Console.Write("  " + geracao[i, j]);
                }
                Console.WriteLine();
            }

            Console.Write("       ");
            for (int j = 0; j < n; j++)
            {
                Console.Write("---");
            }
            Console.WriteLine("--");
        }
    }
}
