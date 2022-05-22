using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;


namespace CineMas.Models
{
    public class ICineBD
    {
        public SqlConnection conexion;
        public string error;

        public ICineBD() {
            this.conexion = ConexionBD.getConexion();
        }

        //obtener listado de Peliculas
        public List<Pelicula> GetPeliculas(int pageIndex, int categoryId, string Search)
        {
            if (pageIndex <= 0) pageIndex = 1;

            List<Pelicula> listPeliculas = new List<Pelicula>();
            SqlCommand comando = new SqlCommand("spGetPeliculas", conexion);
            //comando.CommandText = "select *,(select coalesce(SUM(calificacion),0) from Ranking where (IdPelicula=Peliculas.Id)) as Calificacion,(select coalesce(COUNT(Id),0) from Ranking where (IdPelicula=Peliculas.Id)) as Visto from Peliculas";

            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@PageIndex", pageIndex);
            comando.Parameters.AddWithValue("@CategoriaId", categoryId);
            comando.Parameters.AddWithValue("@Search", Search);

            SqlDataReader registro = comando.ExecuteReader();
            //se instancia la entidad y se lee los registros de la BD
            while (registro.Read())
            {
                Pelicula entity = new Pelicula();
                entity.id = registro.GetInt32(0);
                entity.nombre = registro.GetString(1);
                entity.sinopsis = registro.GetString(2);
                entity.director = registro.GetString(3);
                entity.genero = registro.GetString(4);
                entity.categoriaId = registro.GetInt32(5);
                entity.categoria = registro.GetString(9);
                entity.imgUrl = registro.GetString(6);
                entity.calificacion = registro.GetInt32(7); 
                entity.vistas = registro.GetInt32(8);

                listPeliculas.Add(entity);
            }
            //se cierra la conexion y se retorna el listado
            registro.Close();
            return listPeliculas;
        }

        public Pelicula GetPeliculaById(int idPelicula) 
        {

            Pelicula unaPelicula = new Pelicula();
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "select p.*, c.Categoria from Peliculas p left join Categoria c on c.CategoriaId = p.CategoriaId  where (p.Id=@IdPelicula)";
            comando.Parameters.AddWithValue("@IdPelicula",idPelicula);
            SqlDataReader registro = comando.ExecuteReader();
            //se instancia la entidad y se lee los registros de la BD
            if (registro.Read())
            {
                unaPelicula.id = registro.GetInt32(0);
                unaPelicula.nombre = registro.GetString(1);
                unaPelicula.sinopsis = registro.GetString(2);
                unaPelicula.director = registro.GetString(3);
                unaPelicula.genero = registro.GetString(4);
                unaPelicula.categoriaId = registro.GetInt32(5);
                unaPelicula.categoria = registro.GetString(7);
                unaPelicula.imgUrl = registro.GetString(6);
                registro.Close();
                return unaPelicula;
            }
            else {
                //se cierra la conexion y se retorna el listado
                registro.Close();
                return null;
            }
        }

        public Pelicula GetPeliculaMasVista()
        {

            Pelicula unaPelicula = new Pelicula();
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "select top(1) p.*, r.Vista from ( select r.IdPelicula, Vista = count(r.Id) from Ranking r group by r.IdPelicula ) r join Peliculas p on p.Id = r.IdPelicula order by r.Vista desc ";

            SqlDataReader registro = comando.ExecuteReader();
            //se instancia la entidad y se lee los registros de la BD
            if (registro.Read())
            {
                unaPelicula.id = registro.GetInt32(0);
                unaPelicula.nombre = registro.GetString(1);
                registro.Close();
                return unaPelicula;
            }
            else
            {
                //se cierra la conexion y se retorna el listado
                registro.Close();
                return null;
            }
        }

        public List<Categoria> GetCategorias()
        {

            List<Categoria> listCategorias = new List<Categoria>();
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "select CategoriaId = 0, Categoria = 'Todas las categorías' union all select CategoriaId, Categoria from Categoria where estado = 1 ";

            SqlDataReader registro = comando.ExecuteReader();
            //se instancia la entidad y se lee los registros de la BD
            while (registro.Read())
            {
                Categoria categoria = new Categoria();
                categoria.id = registro.GetInt32(0);
                categoria.nombre = registro.GetString(1);

                listCategorias.Add(categoria);
            }

            registro.Close();
            return listCategorias;
        }


        //Obtener listado de los Rankings
        public List<Ranking> GetRankings()
        {
            List<Ranking> listRanking = new List<Ranking>();
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "select top(5) r.*, p.Nombre from ( select r.IdPelicula, Calificacion = AVG(r.calificacion) from Ranking r group by r.IdPelicula ) r join Peliculas p on p.Id = r.IdPelicula order by r.Calificacion desc ";
            SqlDataReader registro = comando.ExecuteReader();
            //se instancia la entidad y se lee los registros de la BD
            while (registro.Read())
            {
                Ranking entity = new Ranking();
                entity.idPelicula = registro.GetInt32(0);
                entity.valor = registro.GetInt32(1);
                entity.nombrePelicula = registro.GetString(2);
                listRanking.Add(entity);
            }
            //se cierra la conexion y se retorna el listado
            registro.Close();
            return listRanking;
        }

        //guardar calificacion en Rankings
        public bool SaveRanking(Ranking ranking)
        {
            bool save = false;
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion;

            comando.CommandText = "insert into Ranking values(@IdPelicula, @Calificacion)";
            comando.Parameters.AddWithValue("@IdPelicula", ranking.idPelicula);
            comando.Parameters.AddWithValue("@Calificacion", ranking.valor);

            try
            {
                comando.ExecuteNonQuery();
                save = true;
            }
            catch (SqlException e)
            {
                this.error = e.Message;
            }
            return save;
        }

        /*public List<Ranking> GetTotalRankingsValues() 
        {
            List<Ranking> listaRankingValues = new List<Ranking>();
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion;
            comando.CommandText = "select SUM(calificacion) from Ranking where (IdPelicula=IdPelicula)";
            SqlDataReader registro = comando.ExecuteReader();
            //se instancia la entidad y se lee los registros de la BD
            while (registro.Read())
            {
                Ranking entity = new Ranking();
                Pelicula pelicula = new Pelicula();
                entity.sumaValor = registro.GetInt32(0);
                listaRankingValues.Add(entity);
            }
            //se cierra la conexion y se retorna el listado
            registro.Close();
            return listaRankingValues;
        }*/
    }

}