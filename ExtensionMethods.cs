using Color = SplashKitSDK.Color;

namespace Chess
{
    public static class ExtensionMethods
    {
        public static int ReadInteger(this StreamReader reader)
        {
            return Convert.ToInt32(reader.ReadLine());
        }
        public static bool ReadBoolean(this StreamReader reader)
        {
            return Convert.ToBoolean(reader.ReadLine());
        }
        public static float ReadSingle(this StreamReader reader)
        {
            return Convert.ToSingle(reader.ReadLine());
        }
        public static Color ReadColor(this StreamReader reader)
        {
            return Color.RGBColor(reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle()
                );
        }
        public static Coordinate ReadCoordinate(this StreamReader reader)
        {
            return new Coordinate(reader.ReadInteger(), reader.ReadInteger());
        }
        public static Piece ReadPiece(this StreamReader reader)
        {
            
            string pieceKind = reader.ReadLine();
            bool isWhite = reader.ReadBoolean();
            Piece piece = PieceFactory.GetInstance().Operate(pieceKind, isWhite);
            
            return piece;

        }
        public static Cell ReadCell(this StreamReader reader)
        {
            Color clr = reader.ReadColor();
            Coordinate coord = reader.ReadCoordinate();
            Cell cell = new Cell(clr, coord);
            cell.ChessPiece = reader.ReadPiece();
            cell.IsChecked = reader.ReadBoolean();
            cell.IsHighLighted = reader.ReadBoolean();
            return cell;

        }
        public static void WriteColor(this StreamWriter writer, Color clr)
        {
            writer.Write("{0}\n{1}\n{2}\n", clr.R, clr.G, clr.B);
        }
        public static void WriteCoordinate(this StreamWriter writer, Coordinate coord)
        {
            writer.Write("{0}\n{1}\n", coord.X, coord.Y);
        }
        public static void WritePiece(this StreamWriter writer, Piece piece)
        {
            if (piece == null)
            {
                writer.Write("null\nfalse\n");
            } else
            {
                writer.Write("{0}\n{1}\n", piece.Kind.ToString(), piece.IsWhite);
            }
        }
        public static void WriteCell(this StreamWriter writer, Cell cell)
        {
            writer.WriteColor(cell.CellColor);
            writer.WriteCoordinate(cell.Coord);
            writer.WritePiece(cell.ChessPiece);
            writer.WriteLine(cell.IsChecked);
            writer.WriteLine(cell.IsHighLighted);
        }
    }
}
