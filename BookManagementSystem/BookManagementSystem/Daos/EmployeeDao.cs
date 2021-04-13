using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using BookManagementSystem.Models;

namespace BookManagementSystem.Daos
{
    public class EmployeeDao : BaseDao<Employee>
    {
        public EmployeeDao(SqlConnection con) : base(con) { }
        public EmployeeDao(SqlConnection con, SqlTransaction tran) : base(con, tran) { }

        // DB側の名前群
        private const string TABLE_NAME = "employee";
        private const string ID_COLUMN = "id_employee";
        private const string NAME_COLUMN = "nm_employee";
        private const string KANA_COLUMN = "kn_employee";
        private const string MAIL_COLUMN = "mail_address";
        private const string PASS_COLUMN = "password";
        private const string ID_D_COLUMN = "id_department";
        private const string FLG_A_COLUMN = "flg_admin";
        private const string FLG_R_COLUMN = "flg_retirement";
        private const string ID_U_COLUMN = "id_update";
        private const string DATE_U_COLUMN = "date_update";

        public override string GetTableName()
        {
            return TABLE_NAME;
        }

        public override string GetId()
        {
            return ID_COLUMN;
        }

        /// <summary>
        /// 挿入処理
        /// </summary>
        /// <param name="employee">挿入したいデータをセットするエンティティ</param>
        public List<Employee> SelectByName(string name)
        {
            List<Employee> list = null;

            /*
             * 逐次文字列で作成
             * 見やすいが、処理を挟む場合はStringBuilderが良い
             */
            string sql = $@"
                SELECT
                    *

                FROM
                    {this.GetTableName()}
                WHERE
                    {NAME_COLUMN} LIKE @name
            ";
            SqlCommand cmd = new SqlCommand(sql, this.Con);
            SetParameter(cmd, "@name", "%" + name + "%");
            list = ExecuteSelectSql(cmd);
            //list = (List<Employee>)this.Con.Query<Employee>(sql, new Employee { mail_address = mail });

            return list;
        }
        public List<Employee> SelectByMail(string mail)
        {
            List<Employee> list = null;

            /*
             * 逐次文字列で作成
             * 見やすいが、処理を挟む場合はStringBuilderが良い
             */
            string sql = $@"
                SELECT
                    *

                FROM
                    {this.GetTableName()}
                WHERE
                    {MAIL_COLUMN} = @mail_address
            ";
            SqlCommand cmd = new SqlCommand(sql, this.Con);
            SetParameter(cmd, "@mail_address", mail);
            list = ExecuteSelectSql(cmd);
            //list = (List<Employee>)this.Con.Query<Employee>(sql, new Employee { mail_address = mail });

            return list;
        }
        public List<Employee> SelectByDepId(int id)
        {
            List<Employee> list = null;


            string sql = $@"
                SELECT
                    *

                FROM
                    {this.GetTableName()}
                WHERE
                    {ID_D_COLUMN} = @id_department
            ";
            SqlCommand cmd = new SqlCommand(sql, this.Con);
            SetParameter(cmd, "@id_department", id);
            list = ExecuteSelectSql(cmd);
            //list = (List<Employee>)this.Con.Query<Employee>(sql, new Employee { mail_address = mail });

            return list;
        }
       
        public List<Employee> SelectForLogin(string mail, string password)
        {
            List<Employee> list = null;

            string sql = $@"
                SELECT
                    *
                FROM
                    {this.GetTableName()}
                WHERE
                    {MAIL_COLUMN} = @mail 
                     AND 
                        {PASS_COLUMN} = @password
            ";
            SqlCommand cmd = new SqlCommand(sql, this.Con);
            SetParameter(cmd, "@mail", mail);
            SetParameter(cmd, "@password", password);
            list = ExecuteSelectSql(cmd);

            return list;
        }
       
        public int InsertEmployee(Employee employee)
        {
            /* 
             * StringBuilderでSQL作成
             * 性能は良いが、ちょい見にくくなるし書くのが大変
             * AppendLineで書くと改行されて見やすくなる
             */
            var sql = new StringBuilder();
            sql.AppendLine("INSERT INTO " + this.GetTableName());
            sql.AppendLine("(");
            sql.AppendLine("     nm_employee");
            sql.AppendLine("    ,kn_employee");
            sql.AppendLine("    ,mail_address");
            sql.AppendLine("    ,password");
            sql.AppendLine("    ,id_department");
            sql.AppendLine("    ,flg_admin");
            sql.AppendLine("    ,flg_retirement");
            sql.AppendLine("    ,id_update");
            sql.AppendLine(") VALUES (");
            sql.AppendLine("     @name");
            sql.AppendLine("    ,@kana");
            sql.AppendLine("    ,@mail");
            sql.AppendLine("    ,@pass");
            sql.AppendLine("    ,@dep");
            sql.AppendLine("    ,@flg_a");
            sql.AppendLine("    ,@flg_r");
            sql.AppendLine("    ,@id_u");
            sql.AppendLine(")");

            var cmd = new SqlCommand(sql.ToString(), this.Con);
            cmd.Transaction = SqlTran;

            SetParameter(cmd, "@name", employee.nm_employee);
            SetParameter(cmd, "@kana", employee.kn_employee);
            SetParameter(cmd, "@mail", employee.mail_address);
            SetParameter(cmd, "@pass", employee.password);
            SetParameter(cmd, "@dep", employee.id_department);
            SetParameter(cmd, "@flg_a",employee.flg_admin);
            SetParameter(cmd, "@flg_r",employee.flg_retirement);
            SetParameter(cmd, "id_u",employee.id_update);
            SetParameter(cmd, "date_u",employee.date_update.ToString());

            // SQL実行
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="id">更新したいID</param>
        /// <param name="employee">挿入したいデータをセットするエンティティ</param>
        public int UpdatetEmployee(int id, Employee employee)
        {
            string sql = $@"
                UPDATE {this.GetTableName()}
                SET
                     { NAME_COLUMN} = @name
                    ,{ KANA_COLUMN} = @kana
                    ,{ MAIL_COLUMN} = @mail
                    ,{ PASS_COLUMN} = @pass
                    ,{ ID_D_COLUMN} = @dep
                    ,{ FLG_A_COLUMN} = @flg_admin
                    ,{ FLG_R_COLUMN} = @flg_retire
                    ,{ ID_U_COLUMN} = @id_update
                WHERE
                    {ID_COLUMN} = @id
            ";

            var cmd = new SqlCommand(sql.ToString(), this.Con);
            cmd.Transaction = this.SqlTran;

            SetParameter(cmd, "@id", id);
            SetParameter(cmd, "@name", employee.nm_employee);
            SetParameter(cmd, "@kana", employee.kn_employee);
            SetParameter(cmd, "@mail", employee.mail_address);
            SetParameter(cmd, "@pass", employee.password);
            SetParameter(cmd, "@dep", employee.id_department);
            SetParameter(cmd, "@flg_admin", employee.flg_admin);
            SetParameter(cmd, "@flg_retire", employee.flg_retirement);
            SetParameter(cmd, "@id_update", employee.id_update);

            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <param name="id">削除するID</param>
        public void DeleteEmployee(int id)
        {
            string sql = $@"
                DELETE FROM {this.GetTableName()}
                WHERE
                    {ID_COLUMN} = @id
            ";

            var cmd = new SqlCommand(sql.ToString(), this.Con);
            cmd.Transaction = SqlTran;

            SetParameter(cmd, "@id", id);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 1レコードの値からEmployeeのインスタンスを生成する
        /// </summary>
        /// <param name="row">1レコードのデータ</param>
        /// <returns>Employeeのインスタンス</returns>
        protected override Employee RowMapping(SqlDataReader reader)
        {
            Employee emp = new Employee();

            emp.id_employee = (int)reader[ID_COLUMN];
            emp.nm_employee = (string)reader[NAME_COLUMN];
            emp.kn_employee = (string)reader[KANA_COLUMN];
            emp.mail_address = (string)reader[MAIL_COLUMN];
            emp.password = (string)reader[PASS_COLUMN];
            emp.id_department = (int)reader[ID_D_COLUMN];
            emp.flg_admin = (string)reader[FLG_A_COLUMN];
            emp.flg_retirement = (string)reader[FLG_R_COLUMN];
            emp.id_update = (int)reader[ID_U_COLUMN];
            emp.date_update = (DateTime)reader[DATE_U_COLUMN];

            return emp;
        }



    }
}
