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
    public class BookDao : BaseDao<Book>
    {
        public BookDao(SqlConnection con) : base(con) { }
        public BookDao(SqlConnection con, SqlTransaction tran) : base(con, tran) { }

        private const string TABLE_NAME = "book_collection";
        private const string ID_COLUMN = "id_book";
        private const string ISBN_COLUMN = "isbn";
        private const string NAME_COLUMN = "nm_book";
        private const string KANA_COLUMN = "kn_book";
        private const string PUB_COLUMN = "publisher";
        private const string ID_U_COLUMN = "id_update";
        private const string DATE_U_COLUMN = "date_update";
        private const string NOTE_COLUMN = "note";
        private const string FLG_D_COLUMN = "flg_disposal";
        private const string STATUS = "status";

        public override string GetTableName()
        {
            return TABLE_NAME;
        }
        public override string GetId()
        {
            return ID_COLUMN;
        }
        public new List<Book> SelectAll()
        {
            List<Book> list = null;

            string sql = $@"
                SELECT 
                    * 
                FROM 
                    {this.GetTableName()} LEFT JOIN books
                        ON {this.GetTableName()}.isbn = books.isbn 
                    LEFT JOIN borrow_books 
                        ON {this.GetTableName()}.id_book = borrow_books.id_book
            ";
            SqlCommand cmd = new SqlCommand(sql, this.Con);
            list = ExecuteSelectSql(cmd);

            return list;
        }
        public List<Book> SelectByName(string name)
        {
            List<Book> list = null;

            string sql = $@"
                SELECT 
                    *
                FROM
                    {this.GetTableName()} LEFT JOIN books
                        ON {this.GetTableName()}.isbn = books.isbn 
                    LEFT JOIN borrow_books 
                        ON {this.GetTableName()}.id_book = borrow_books.id_book
                WHERE
                    {NAME_COLUMN} LIKE @nm_book
           ";

            SqlCommand cmd = new SqlCommand(sql, this.Con);
            SetParameter(cmd, "@nm_book", "%"+ name +"%");
            list = ExecuteSelectSql(cmd);

            return list;
        }
        public List<Book> SelectByPub(string publisher)
        {
            List<Book> list = null;

            string sql = $@"
                SELECT 
                    *
                FROM
                    {this.GetTableName()} LEFT JOIN books
                        ON {this.GetTableName()}.isbn = books.isbn 
                    LEFT JOIN borrow_books 
                        ON {this.GetTableName()}.id_book = borrow_books.id_book
                WHERE
                    {PUB_COLUMN} LIKE @publisher
           ";

            SqlCommand cmd = new SqlCommand(sql, this.Con);
            SetParameter(cmd, "@publisher", "%" + publisher + "%");
            list = ExecuteSelectSql(cmd);

            return list;
        }

        public List<Book> SelectByNote(string note)
        {
            List<Book> list = null;

            string sql = $@"
                SELECT 
                    * 
                FROM 
                    {this.GetTableName()} LEFT JOIN books
                        ON {this.GetTableName()}.isbn = books.isbn 
                    LEFT JOIN borrow_books 
                        ON {this.GetTableName()}.id_book = borrow_books.id_book
                WHERE 
                    CONVERT(VARCHAR(20), {NOTE_COLUMN}) LIKE @note;               
            ";
            SqlCommand cmd = new SqlCommand(sql, this.Con);
            SetParameter(cmd, "@note", "%" + note + "%");
            list = ExecuteSelectSql(cmd);

            return list;
        }
        public new List<Book> SelectById(int id)
        {
            List<Book> list = null;

            string sql = $@"
                SELECT 
                    * 
                FROM 
                    {this.GetTableName()} LEFT JOIN books
                        ON {this.GetTableName()}.isbn = books.isbn 
                    LEFT JOIN borrow_books 
                        ON {this.GetTableName()}.id_book = borrow_books.id_book
                WHERE 
                    {this.GetTableName()}.{ID_COLUMN} = @id_book;               
            ";
            SqlCommand cmd = new SqlCommand(sql, this.Con);
            SetParameter(cmd, "@id_book", id);
            list = ExecuteSelectSql(cmd);

            return list;
        }

        public int InsertBook(Book book)
        {
            var sql = new StringBuilder();
            sql.AppendLine("INSERT INTO books" );
            sql.AppendLine("(");
            sql.AppendLine("     isbn");
            sql.AppendLine("    ,nm_book");
            sql.AppendLine("    ,kn_book");
            sql.AppendLine("    ,publisher");
            sql.AppendLine("    ,id_update");
            sql.AppendLine(") VALUES (");
            sql.AppendLine("     @isbn");
            sql.AppendLine("    ,@name");
            sql.AppendLine("    ,@kana");
            sql.AppendLine("    ,@pub");
            sql.AppendLine("    ,@id_u");
            sql.AppendLine(")");
            sql.AppendLine("INSERT INTO " + this.GetTableName());
            sql.AppendLine("(");
            sql.AppendLine("     isbn");
            sql.AppendLine("    ,note");
            sql.AppendLine("    ,flg_disposal");
            sql.AppendLine("    ,id_update");
            sql.AppendLine(") VALUES (");
            sql.AppendLine("     @isbn");
            sql.AppendLine("    ,@note");
            sql.AppendLine("    ,@flg_d");
            sql.AppendLine("    ,@id_u");
            sql.AppendLine(")");

            var cmd = new SqlCommand(sql.ToString(), this.Con);
            cmd.Transaction = SqlTran;

            SetParameter(cmd, "@isbn", book.isbn);
            SetParameter(cmd, "@name", book.nm_book);
            SetParameter(cmd, "@kana", book.kn_book);
            SetParameter(cmd, "@pub", book.publisher);
            SetParameter(cmd, "@id_u", book.id_update);
            SetParameter(cmd, "@flg_d", book.flg_disposal);
            SetParameter(cmd, "@note", book.note);

            return cmd.ExecuteNonQuery();
        }

        public int UpdateBook(int id, Book book)
        {
            string sql = $@"
                UPDATE books
                SET
                     { NAME_COLUMN} = @name
                    ,{ KANA_COLUMN} = @kana
                    ,{ PUB_COLUMN} = @pub
                    ,{ ID_U_COLUMN} = @id_u
                FROM 
                    books INNER JOIN book_collection ON (books.isbn = book_collection.isbn) 
                WHERE
                    {this.GetTableName()}.{ID_COLUMN} = @id
                
                UPDATE {this.GetTableName()}
                SET 
                     {NOTE_COLUMN} = @note
                    ,{FLG_D_COLUMN} = @flg_d
                    ,{ID_U_COLUMN} = @id_u
                WHERE
                    {ID_COLUMN} = @id
            ";

            var cmd = new SqlCommand(sql.ToString(), this.Con);
            cmd.Transaction = this.SqlTran;

            if(book.kn_book == null)
            {
                book.kn_book = "";
            }
            if (book.note == null)
            {
                book.note = "";
            }

            SetParameter(cmd, "@id", id);
            SetParameter(cmd, "@name", book.nm_book);
            SetParameter(cmd, "@kana", book.kn_book);
            SetParameter(cmd, "@pub", book.publisher);
            SetParameter(cmd, "@id_u", book.id_update);
            SetParameter(cmd, "@flg_d", book.flg_disposal);
            SetParameter(cmd, "@note", book.note);

            return cmd.ExecuteNonQuery();
        }
        protected override Book RowMapping(SqlDataReader reader)
        {
            Book book = new Book();

            book.id_book = (int)reader[ID_COLUMN];
            book.isbn = (string)reader[ISBN_COLUMN];
            book.nm_book = (string)reader[NAME_COLUMN];
            book.kn_book = Convert.IsDBNull(reader[KANA_COLUMN]) ? null: (string) reader[KANA_COLUMN];
            book.publisher = (string)reader[PUB_COLUMN];
            book.id_update = (int)reader[ID_U_COLUMN];
            book.date_update = (DateTime)reader[DATE_U_COLUMN];
            book.note = Convert.IsDBNull(reader[NOTE_COLUMN]) ? null : (string)reader[NOTE_COLUMN];
            book.flg_disposal = (string)reader[FLG_D_COLUMN];
            book.status = Convert.IsDBNull(reader[STATUS]) ? "1" : (string)reader[STATUS];

            return book;
        }
    }
}
