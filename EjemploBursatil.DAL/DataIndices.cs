using EjemploBursatil.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploBursatil.DAL
{
    public class DataIndices
    {
        public void AgregarIndice(Indices indice)
        {
            using (BursatilEntities context= new BursatilEntities())
            {
                //Todo: Validacion que no exista ya ( o lo pise, tenemos que ver negocio)
               
                context.Indices.Add(indice);
                context.SaveChanges();
            }

        }

        public void Borrar()
        {
            using (BursatilEntities context = new BursatilEntities())
            {
                context.Database.ExecuteSqlCommand("DELETE FROM INDICES");
            }
        }

        public List<Indices> TraerTodos()
        {
            List<Indices> lista;
            using (BursatilEntities context = new BursatilEntities())
            {
                //Todo: Validacion que no exista ya ( o lo pise, tenemos que ver negocio)
                lista= context.Indices.ToList();
                ///(context.SaveChanges();
            }
            return lista;
        }

        public List<Recomendacion> ObtenerRecomendaciones(List<Indices> listaIndices)
        {
            List<Recomendacion> lista = new List<Recomendacion>();
            //UP DOWN EQ
            //TENDENCIA ALZA 1 , MEDIA 0 O BAJA -1
            Recomendacion recomLineal= new Recomendacion();
            recomLineal.Tendencia = ObtenerTendencia(listaIndices);
            recomLineal.Texto = string.Format("Segun la tendencia Lineal ({0})", recomLineal.Tendencia);
            
            lista.Add(recomLineal);

            Recomendacion recomStock = new Recomendacion();
            recomStock.Tendencia = ObtenerTendenciaStock(listaIndices);
            recomStock.Texto = string.Format("Segun Oscilador Estocastico({0})", recomStock.Tendencia);
            lista.Add(recomStock);

            Recomendacion recomRSI = new Recomendacion();
            recomRSI.Tendencia = ObtenerTendenciaRSI(listaIndices);
            recomRSI.Texto = string.Format("Segun Indicador RSI ({0})", recomRSI.Tendencia);
            lista.Add(recomRSI);


            Recomendacion recomFinal = new Recomendacion();
            recomFinal.Tendencia = ObtenerRecomFinal(listaIndices, recomLineal.Tendencia, recomStock.Tendencia, recomRSI.Tendencia);
            recomFinal.Texto = string.Format("Como Resultado Final se Predice => ");
            lista.Add(recomFinal);

            return lista;
        }

        private int ObtenerRecomFinal(List<Indices> listaIndices, int tendLineal, int tendStock, int tendRSI)
        {
            //Aca tengo que ponderar todo y dar un pronostico
            return 1;
        }

       

       
        private int ObtenerTendenciaRSI(List<Indices> listaIndices)
        {
            int rta = 0;
            foreach (Indices item in listaIndices.OrderByDescending(o => o.Fecha).Take(3))
            {
                if (item.RSI < 30)
                {
                    return 1;
                }
                else
                {
                    if (item.RSI>70)
                    {
                        return -1;
                    }
                }
            }
            return rta;

        }

        private int ObtenerTendenciaStock(List<Indices> listaIndices)
        {
            //Debo detectar un cruze en los ultimos 3 dias
            double valorAntStock3 = 0;
            double valorAntStock6 = 0;
            foreach (Indices item in listaIndices.OrderByDescending(o=>o.Fecha).Take(3))
            {
                if (valorAntStock3!=0)
                {
                    if (valorAntStock3 > valorAntStock6)
                    {
                        if (item.Stock3 < item.Stock6)
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        if (item.Stock3 > item.Stock6)
                        {
                            return -1; //TODO: probar esto
                        }
                    }
                }
                valorAntStock3 = item.Stock3;
                valorAntStock6 = item.Stock6;
            }
            return 0;
            
        }

        private int ObtenerTendencia(List<Indices> listaIndices)
        {
            if (listaIndices[0].Tendencia == listaIndices[1].Tendencia)
            {
                return 0;
            }
            else
            {
                if (listaIndices[0].Tendencia < listaIndices[1].Tendencia)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }

        public string ObtenerIcono(string mensaje)
        {
            return mensaje.Split(':')[1].Trim();
        }
    }
}
