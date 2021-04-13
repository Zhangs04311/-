using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using BookManagementSystem.Models;

namespace BookManagementSystem.Daos
{
    public class BorrowDao:BaseDao<Borrow>
    {
        public BorrowDao(SqlConnection con) : base(con) { }
        public BorrowDao(SqlConnection con, SqlTransaction tran) : base(con, tran) { }

        private const string TABLE_NAME = "borrow_books";
        private const string ID_COLUMN = "id_request";
        private const string ID_E_COLUMN = "id_employee";
        private const string ID_B_COLUMN = "id_book";
        private const string DATE_REQ_COLUMN = "date_request";
        private const string STATUS_COLUMN = "status";
        private const string ID_A_COLUMN = "id_approval";
        private const string DATE_A_COLUMN = "date_approval";
        private const string DATE_B_COLUMN = "date_borrow";
        private const string DATE_COLUMN = "date";
        private const string DATE_RET_COLUMN = "date_return";
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
        public void InsertBorrow(Borrow borrow)
        {
            var sql = new StringBuilder();
            sql.AppendLine("INSERT INTO " + this.GetTableName());
            sql.AppendLine("(");
            sql.AppendLine("     id_employee");
            sql.AppendLine("    ,id_book");
            sql.AppendLine("    ,date_request");
            sql.AppendLine("    ,status");          
            sql.AppendLine("    ,date_borrow");
            sql.AppendLine("    ,date");
            sql.AppendLine("    ,id_update");
            sql.AppendLine(") VALUES (");
            sql.AppendLine("     @empId");
            sql.AppendLine("    ,@bookId");
            sql.AppendLine("    ,@reqDate");
            sql.AppendLine("    ,'0'");
            sql.AppendLine("    ,@borrowDate");
            sql.AppendLine("    ,@date");
            sql.AppendLine("    ,1");
            sql.AppendLine(")");

            var cmd = new SqlCommand(sql.ToString(), this.Con);
            cmd.Transaction = SqlTran;

            SetParameter(cmd, "@empId", borrow.id_employee);
            SetParameter(cmd, "@bookId", borrow.id_book);
            SetParameter(cmd, "@reqDate", DateTime.Now.ToString());
            SetParameter(cmd, "borrowDate", borrow.date_borrow.ToString());
            SetParameter(cmd, "date", borrow.date.ToString());

            cmd.ExecuteNonQuery();
        }
        public List<Borrow> SelectForApproval()
        {
            List<Borrow> list = null;

            string sql = $@"
                SELECT 
                    *
                FROM
                    {this.GetTableName()}
                WHERE
                    {STATUS_COLUMN} = '0'
            ";
            SqlCommand cmd = new SqlCommand(sql, this.Con);
            list = ExecuteSelectSql(cmd);

            return list;

        }
        public int UpdateForApproval(List<int> id)
        {
            var sql = new StringBuilder();
            sql.AppendLine("UPDATE " + this.GetTableName());
            sql.AppendLine(" SET");
            sql.AppendLine("    status = '2'");
            sql.AppendLine("WHERE");
            sql.AppendLine("    id_request IN");
            sql.AppendLine("    (");

            int current = 1;
            foreach(var item in id)
            {
                if(current == id.Count)
                    sql.AppendLine("     " + item.ToString());
                else
                    sql.AppendLine("     " + item.ToString() + ",");

                current ++;
            }
            sql.AppendLine("    )");

            var cmd = new SqlCommand(sql.ToString(), this.Con);
            cmd.Transaction = SqlTran;

            return cmd.ExecuteNonQuery();
        }
        protected override Borrow RowMapping(SqlDataReader reader)
        {
            Borrow borrow = new Borrow();

            borrow.id_request = (int)reader[ID_COLUMN];
            borrow.id_employee = (int)reader[ID_E_COLUMN];
            borrow.id_book = (int)reader[ID_B_COLUMN];
            borrow.date_request = (DateTime)reader[DATE_REQ_COLUMN];
            borrow.status = (string)reader[STATUS_COLUMN];
            borrow.id_approval = Convert.IsDBNull(reader[ID_A_COLUMN]) ? null:(int?)reader[ID_A_COLUMN];
            borrow.date_approval = Convert.IsDBNull(reader[DATE_A_COLUMN]) ? null:(DateTime?)reader[DATE_A_COLUMN];
            borrow.date_borrow = Convert.IsDBNull(reader[DATE_B_COLUMN]) ? null : (DateTime?)reader[DATE_B_COLUMN];
            borrow.date = Convert.IsDBNull(reader[DATE_COLUMN]) ? null : (DateTime?)reader[DATE_COLUMN];
            borrow.date_return = Convert.IsDBNull(reader[DATE_RET_COLUMN]) ? null : (DateTime?)reader[DATE_RET_COLUMN];
            borrow.id_update = (int)reader[ID_U_COLUMN];
            borrow.date_update = (DateTime)reader[DATE_U_COLUMN];

            return borrow;
        }
    }
}
