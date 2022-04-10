using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;
using System.Data;
using System.Data.SQLite;




class Uitls{

    public static Dictionary<string, object> ValidarUsuario(string Usuario, string Clave){

        Clave = Conexion.CreateMD5(Clave);
        var result = new Dictionary<string, object>();
        var sql = "Select * From usuario Where usuarios = @Usuario AND clave = @Clave";
        var parametros = new Dictionary<string, object>();
        parametros.Add("@Usuario", Usuario);
        parametros.Add("@Clave",Clave);
        var dt = Conexion.consulta(sql, parametros);
        if(dt.Count > 0 ){
            result = dt[0];
        }
        return result;
    }

}
class Conexion
{

    SQLiteConnection oCon = new SQLiteConnection();

    static Conexion instancia = null;
    public static SQLiteConnection getInstancia()
    {

        if (instancia == null)
        {

            instancia = new Conexion();

        }
        return instancia.oCon;

    }

    public static List<Dictionary<string, object>> consulta(string sql)
    {

        List<Dictionary<string, object>> lista = new List<Dictionary<string, object>>();
        SQLiteCommand oCom = new SQLiteCommand(sql, getInstancia());

        SQLiteDataAdapter oAdap = new SQLiteDataAdapter(oCom);
        DataTable dt = new DataTable();
        oAdap.Fill(dt);

        foreach (DataRow row in dt.Rows)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                dic.Add(col.ColumnName, row[col]);
            }
            lista.Add(dic);
        }
        return lista;
    }

     public static List<Dictionary<string, object>> consulta(string sql, Dictionary<string, object> parametros)
    {

        List<Dictionary<string, object>> lista = new List<Dictionary<string, object>>();

        SQLiteCommand oCom = new SQLiteCommand(sql, getInstancia());

        if(parametros.Count > 0){
            foreach(var param in parametros){
                oCom.Parameters.AddWithValue(param.Key, param.Value);
            }

        }

        SQLiteDataAdapter oAdap = new SQLiteDataAdapter(oCom);
        DataTable dt = new DataTable();
        oAdap.Fill(dt);

        foreach (DataRow row in dt.Rows)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                dic.Add(col.ColumnName, row[col]);
            }
            lista.Add(dic);
        }
        return lista;
    }

    
    public static void insertar(string table, Dictionary<string, object> dic)
    {
        string sql = " INSERT INTO " + table + "(";
        string valores = "";
        string campos = "";

        foreach (KeyValuePair<string, object> kvp in dic)
        {
            campos += kvp.Key + ",";
            valores += "@" + kvp.Key + ",";
        }
        campos = campos.Substring(0, campos.Length - 1);
        valores = valores.Substring(0, valores.Length - 1);

        sql += campos + ") values(" + valores + ")";

        Console.WriteLine(sql);

        SQLiteCommand oCom = new SQLiteCommand(sql, getInstancia());
        foreach (KeyValuePair<string, object> kvp in dic)
        {
            oCom.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
        }
        oCom.ExecuteNonQuery();
    }


    public static void Ejecutar(string sql){
        SQLiteCommand oCom = new SQLiteCommand(sql, getInstancia());
        oCom.ExecuteScalar();
    }

    public static bool Actualizar(string tabla, Dictionary<string, object> datos, string campoPrimario){
        
        bool rsx = true;
        List<string> campos = new List<string>();
        foreach(var item in datos){
            if(item.Key != campoPrimario){
            campos.Add("`" +item.Key+"` = $"+item.Key);
            }
        }
        string detalle = string.Join(",", campos);

        var sql = " UPDATE "  +  tabla  + " SET " +  detalle  + " WHERE " + campoPrimario + "  =  "  +  datos  [ campoPrimario ] ; 
        //Console.WriteLine(sql);

        SQLiteCommand oCom = new SQLiteCommand(sql, Conexion.getInstancia());
        foreach(var item in datos){
            if (item.Key != campoPrimario){
                oCom.Parameters.AddWithValue("$"+item.Key, item.Value);
            }
        }
        try{
            oCom.ExecuteNonQuery();
            Console.WriteLine(" Actualizar correctamente ");

        }catch (Exception ex){
            rsx = false;
            Console.WriteLine(ex.Message);

        }

        return rsx;
    } 

    Conexion()
    {
        
        oCon.ConnectionString = "Data Source=Bufedb.db;Version=3;";
        oCon.Open();
    }
    ~Conexion()
    {
        oCon.Close();
    }
    public static string CreateMD5(string input)
    {
        input = input + "Vamos a ver si adivina esto @$#$#(*&$)sjkhqkjkejiruq!!!klkljlkd" ;
        // Use input string to calculate MD5 hash
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);

            
        }
    }

}
public enum ModoApp
{
    Lista,
    Normal,
    Insertar,
    Modificar,
    Eliminar,
}

