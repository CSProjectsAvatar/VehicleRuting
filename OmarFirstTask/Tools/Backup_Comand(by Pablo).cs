using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OmarFirstTask.Route;

namespace OmarFirstTask
{
    public abstract class Comand { }
    public abstract class NoIterableComand : Comand
    {
        public abstract void Execute();        
    }
    public abstract class IterableComand : Comand, IEnumerable<object>
    {
        public abstract IEnumerator<object> GetEnumerator();


        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class SelectRoute : IterableComand
    {
        private DistributionNetwork dn;
        public SelectRoute(DistributionNetwork dn)
        {
            this.dn = dn;
        }
        //Enumerator No Random
        public override IEnumerator<object> GetEnumerator()
        {
            foreach (var item in dn.Vehicles)
            {
                yield return item.Route;
            }

        }
        //Enumerator Random
        public IEnumerator<object> GetRandomEnumerator()
        {
            var auxiliarList = dn.Vehicles.ToList();//Revisar
            Random random = new Random();
            for (int i = dn.Vehicles.Count; i >= 0; i--)
            {
                int index = random.Next(i);//aqui tengo duda en si el no va a voler a sakr el mismo numero
                var a = auxiliarList[index];
                auxiliarList.RemoveAt(index);
                yield return a;
            }
           

        }
    }

    public class InsertClient : NoIterableComand
    {
        private Client client;
        private Route route;
        private List<int> indexer;// sto pa hacer el random insert
        private int lastIndex;
        public InsertClient(Client client, Route route)
        {
            this.client = client;
            this.route = route;
            for (int i = 0; i < route.clientList.Count; i++)
            {
                indexer.Add(i);
            }
            lastIndex = -1;
        }

        public override void Execute()
        {
            if (lastIndex >= 0)// hay que quitar lo que habia hecho antes
                CleanLastExecute();//me falta q no llegue al count
            route.clientList.Insert(lastIndex + 1, client); // este siempre pone en el siguiente a lo que habia puesto
            lastIndex++;
        }

        public  void RandomExecute()
        {
            if (lastIndex >= 0)// hay que quitar lo que habia hecho antes
                CleanLastExecute();
            Random random = new Random(indexer.Count);
            int index = random.Next();
            lastIndex = indexer[index];
            route.clientList.Insert(lastIndex, client);
            indexer.RemoveAt(index);

        }

        protected void CleanLastExecute()
        {
            route.clientList.RemoveAt(lastIndex);
        }


    }
    public class SelectSubroute : IterableComand
    {
        private Routs rout;
        public SelectSubrout(Routs route)
        {
            this.rout = rout;
        }
        public override IEnumerator<object> GetEnumerator()
        {
            List<Client> subRout = new List<Client>();
            for (int l = 1; l < rout.length; l++)//ver implementacion de ruta voy a tomar
            {
                for (int i = 0; i < rout.length - l; l++)
                {
                    //no recuerdo como concatenar deos listas
                    // por ahora esto 
                    for (int k = 0; k < l; k++)
                    {
                        subRout.Add(rout[k]);
                    }

                    yield return subRout;
                }
            }
        }
        //denme ideas de como es hacer esto random

    }
    public class SwapSubroute : NoIterableComand
    {
        private Route rout1;
        private int begining1;
        private int large1;
        private Route rout2;
        private int begining2;
        private int large2;
        public SwapSubroute(Route rout1, int begining1,int large1, Route rout2, int begining2, int large2)
        {
            this.begining1 = begining1;
            this.begining2 = begining2;
            this.rout1 = rout1;
            this.rout2 = rout2;
            this.large1 = large1;
            this.large2 = large2;
        }
        public override void Execute()
        {
            List<Client> auxiliary = new List<Client>();
            for (int i = begining1; i < large1; i++)
            {
                auxiliary.Add(rout1.clientList[begining1]);
                rout1.clientList.RemoveAt(begining1);
            }
            for (int i = begining2; i < large2; i++)
            {
                rout1.clientList.Insert(begining1 + i - begining2, rout2.clientList[begining2]);
                rout2.clientList.RemoveAt(begining2);
                
            }
            for (int i = 0; i < large1; i++)
            {
                rout2.clientList.Insert(begining2 + i, auxiliary[i]);
            }
        }
        
    }
    public class SwapClients : NoIterableComand
    {
        private Route rout1;
        private int index1;
        private Route rout2;
        private int index2;

        public SwapClients(Route rout1, int index1, Route rout2, int index2)
        {
            this.rout1 = rout1;
            this.index1 = index1;
            this.rout2 = rout2;
            this.index2 = index2;


        }
        public override void Execute()
        {
            Client temp = rout1.clientList[index1];
            rout1.clientList[index1] = rout2.clientList[index2];
            rout2.clientList[index2] = temp;
        }
        
    }
    public class SelectClient : IterableComand
    {
        private Route route;
        private List<int> indexer;
        public SelectClient(Route route) {
            this.route = route;
        }
        public override IEnumerator<object> GetEnumerator()
        {
            return route.clientList.GetEnumerator();
        }
        public IEnumerator<object> GetRandomEnumerator()
        {
            Random random = new Random();
            for (int i = 0; i < route.clientList.Count; i++)
            {
                Random random = 
            }
            Random random = new random
        }
    }

    //preguntar a omar si esto hay que hacerle un ve a atras
    //public class SwapClients:Comand
    //{
    //    private DistributionNetwork dn;
    //    public SelectRoute(DistributionNetwork dn)
    //    {
    //        this.dn = dn;
    //    }

    //    //Enumerator No Random
    //    public override IEnumerator<object> GetEnumerator()
    //    {
    //        foreach (var item in dn.Vehicles)
    //        {
    //            yield return item;
    //        }
    //    }

    //    //Enumerator Random
    //    public IEnumerator<object> GetRandomEnumerator()
    //    {
    //        var auxiliarList = dn.Vehicles.ToList();//Revisar

    //        for (int i = auxiliarList.Count - 1; i >= 0; i--)
    //        {
    //            var a = auxiliarList[i];
    //            auxiliarList.RemoveAt(i);
    //            yield return a;
    //        }
    //    }

    //}
}