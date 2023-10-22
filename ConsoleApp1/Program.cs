// See https://aka.ms/new-console-template for more information
using ConsoleApp1.DBs;
using S7.Net;
using S7.Net.Types;

Console.WriteLine("Hello, World!");

Console.WriteLine("Wreating Value to PLC!");




// Set Value in DB

int value = 1; // 1 - set value single Variable in db, 2 - set all db with Varibles , 3 - reset value in db


using (var plc = new Plc(CpuType.S71200, "192.168.0.1", 0, 1))
{
    plc.Open();

    if (value == 1)
    {
        Console.WriteLine("\n--- DB 1 SET single Variable ---\n");
        plc.Write("DB1.DBX0.0", true);
        plc.Write("DB1.DBX0.1", true);

        short db1IntVariable = 50;
        plc.Write("DB1.DBW2.0", db1IntVariable.ConvertToUshort());

        double db1RealVariable = 35.46;
        plc.Write("DB1.DBD4.0", Convert.ToUInt32(db1RealVariable));

        int db1DintVariable = 987654;
        plc.Write("DB1.DBD8.0", db1DintVariable.ConvertToUInt());
    
        int db1DwordVariable = 654321;
        plc.Write("DB1.DBD12.0", db1DwordVariable.ConvertToUInt());

        short db1WordVariable = 321;
        plc.Write("DB1.DBW16.0", Convert.ToUInt16(db1WordVariable));



        Console.WriteLine("\n\n\n\n");

        Console.WriteLine("\n--- DB 3 SET ---\n");
        plc.Write("DB3.DBX0.0", true);
        plc.Write("DB3.DBX0.1", true);

        short db3IntVariable = 50;
        plc.Write("DB3.DBW2.0", db3IntVariable.ConvertToUshort());

        double db3RealVariable = 35.46;
        plc.Write("DB3.DBD4.0", Convert.ToUInt32(db3RealVariable));

        int db3DintVariable = 987654;
        plc.Write("DB3.DBD8.0", db3DintVariable.ConvertToUInt());

        int db3DwordVariable = 654321;
        plc.Write("DB1.DBD12.0", db3DwordVariable.ConvertToUInt());

        short db3WordVariable = 321;
        plc.Write("DB3.DBW16.0", Convert.ToUInt16(db3WordVariable));
    }
    else if(value == 2)
    {

        Console.WriteLine("\n--- DB 1 SET All Block ---\n");

        byte[] db1Bytes = new byte[18];

        S7.Net.Types.Boolean.SetBit(db1Bytes[0], 0);    // DB1.DBX0.0
        S7.Net.Types.Boolean.SetBit(db1Bytes[0], 1);    // DB1.DBX0.1

        short db1IntVariable = 50;
        S7.Net.Types.Int.ToByteArray(db1IntVariable).CopyTo(db1Bytes, 2);   // DB1.DBW2.0

        double db1RealVariable = 35.46;
        S7.Net.Types.Double.ToByteArray(db1RealVariable).CopyTo(db1Bytes, 4);   // DB1.DBD4.0

        int db1DintVariable = 987654;
        S7.Net.Types.DInt.ToByteArray(db1DintVariable).CopyTo(db1Bytes, 8);   // DB1.DBD8.0

        uint db1DwordVariable = 654321;
        S7.Net.Types.DWord.ToByteArray(db1DwordVariable).CopyTo(db1Bytes, 12);   // DB1.DBD12.0

        ushort db1WordVariable = 321;
        S7.Net.Types.Word.ToByteArray(db1WordVariable).CopyTo(db1Bytes, 16);   // DB1.DBD16.0
    }
    else if(value == 3)
    {
        //Reset DB
        Console.WriteLine("\n--- DB 1 RESET ---\n");
        byte[] db1Bytes = new byte[18];
        byte[] db3Bytes = new byte[18];


        plc.WriteBytes(DataType.DataBlock, 1, 0, db1Bytes);
        plc.WriteBytes(DataType.DataBlock, 3, 0, db3Bytes);


    }


    //Reset DB
    //byte[] db1Bytes = new byte[18];
    //byte[] db3Bytes = new byte[18];


    //plc.WriteBytes(DataType.DataBlock, 1, 0, db1Bytes);
    //plc.WriteBytes(DataType.DataBlock, 3, 0, db3Bytes);


    //20m55s
}



Console.WriteLine("Reading Value Solution1!\n\n\n");
using (var plc = new Plc(CpuType.S71200, "192.168.0.1", 0, 1))
{
    plc.Open();

    List<DataItem> dataItems = new List<DataItem>
    {
        new DataItem()
        {
            DataType = DataType.DataBlock,
            DB = 1,
            Count = 38,
        },
        new DataItem()
        {
            DataType = DataType.DataBlock,
            DB = 3,
            Count = 22, 
        }
    };
    plc.ReadMultipleVars(dataItems);

    var db1Bytes = dataItems[0].Value as byte[];
    var db3Bytes = dataItems[1].Value as byte[];


    //    Console.WriteLine("\n--- DB 1 ---\n");

    bool db1Bool1 = db1Bytes[0].SelectBit(0);
    Console.WriteLine("DB1.DBX0.0: " + db1Bool1);

    bool db1Bool2 = db1Bytes[0].SelectBit(1);
    Console.WriteLine("DB1.DBX0.1: " + db1Bool2);

    short db1IntVariable = S7.Net.Types.Int.FromByteArray(db1Bytes.Skip(2).Take(2).ToArray());
    Console.WriteLine("DB1.DBW2.0" + db1IntVariable);

    double db1RealVariable = S7.Net.Types.Double.FromByteArray(db1Bytes.Skip(4).Take(4).ToArray());
    Console.WriteLine("DB1.DBD4.0" + db1RealVariable);

    int db1DintVariable = S7.Net.Types.DInt.FromByteArray(db1Bytes.Skip(8).Take(4).ToArray());
    Console.WriteLine("DB1.DBD8.0" + db1DintVariable);

    uint db1DwordVariable = S7.Net.Types.DWord.FromByteArray(db1Bytes.Skip(12).Take(4).ToArray());
    Console.WriteLine("Db1.DBD12.0 " + db1DwordVariable);

    ushort db1WordVariable = S7.Net.Types.Word.FromByteArray(db1Bytes.Skip(16).Take(2).ToArray());
    Console.WriteLine("DB1.DBW16.0: " + db1WordVariable);

//    Console.WriteLine("\n--- DB 3 ---\n");


    bool db3Bool1 = db3Bytes[0].SelectBit(0);
    Console.WriteLine("DB3.DBX0.0: " + db3Bool1);

    bool db3Bool2 = db3Bytes[0].SelectBit(1);
    Console.WriteLine("DB3.DBX0.1: " + db3Bool2);

    short db3IntVariable = S7.Net.Types.Int.FromByteArray(db3Bytes.Skip(2).Take(2).ToArray());
    Console.WriteLine("DB3.DBW2.0" + db3IntVariable);

    double db3RealVariable = S7.Net.Types.Double.FromByteArray(db3Bytes.Skip(4).Take(4).ToArray());
    Console.WriteLine("DB3.DBD4.0" + db3RealVariable);

    int db3DintVariable = S7.Net.Types.DInt.FromByteArray(db3Bytes.Skip(8).Take(4).ToArray());
    Console.WriteLine("DB3.DBD8.0" + db3DintVariable);

    uint db3DwordVariable = S7.Net.Types.DWord.FromByteArray(db3Bytes.Skip(12).Take(4).ToArray());
    Console.WriteLine("Db3.DBD12.0 " + db3DwordVariable);

    ushort db3WordVariable = S7.Net.Types.Word.FromByteArray(db3Bytes.Skip(16).Take(2).ToArray());
    Console.WriteLine("DB3.DBW14.0: " + db3WordVariable);

    double db3RealVariable2 = S7.Net.Types.Double.FromByteArray(db3Bytes.Skip(18).Take(4).ToArray());
    Console.WriteLine("DB2.DBD18.0" + db3RealVariable2);
}



Console.WriteLine("\n\n\n\n\n\n");


Console.WriteLine("Reading Value Solution3!\n\n");

using (var plc = new Plc(CpuType.S71200, "192.168.0.1", 0, 1))
            {
    plc.Open();

    //bool db1Bool1 = (bool)plc.Read("DB1.DBX0.0");
    //Console.WriteLine("DB1.DBX0.0: " + db1Bool1);

    //bool db1Bool2 = (bool)plc.Read("DB1.DBX0.1");
    //Console.WriteLine("DB1.DBX0.1: " + db1Bool2);

    //var db1IntVariable = (ushort)plc.Read("DB1.DBW2.0");
    //Console.WriteLine("DB1.DBW2.0: " + db1IntVariable);

    //var db1RealVariable = ((uint)plc.Read("DB1.DBD4.0"));
    //Console.WriteLine("DB1.DBD4.0: " + db1RealVariable);

    //var db1DintVariable = (uint)plc.Read("DB1.DBD8.0");
    //Console.WriteLine("DB1.DBD8.0: " + db1DintVariable);

    //var db1DwordVariable = (uint)plc.Read("DB1.DBD12.0");
    //Console.WriteLine("DB1.DBD12.0: " + db1DwordVariable);

    //var db1WordVariable = (ushort)plc.Read("DB1.DBW16.0");
    //Console.WriteLine("DB1.DBD16.0: " + db1WordVariable);




    Console.WriteLine("\n--- DB 1 ---\n");

    var db1Bytes = plc.ReadBytes(DataType.DataBlock, 1, 0, 38);
    Console.WriteLine("ReadBytes DB1 Bytes" + db1Bytes);

    bool db1Bool1 = db1Bytes[0].SelectBit(0);
    Console.WriteLine("DB1.DBX0.0: " + db1Bool1);

    bool db1Bool2 = db1Bytes[0].SelectBit(1);
    Console.WriteLine("DB1.DBX0.1: " + db1Bool2);

    short db1IntVariable = S7.Net.Types.Int.FromByteArray(db1Bytes.Skip(2).Take(2).ToArray());
    Console.WriteLine("DB1.DBW2.0" + db1IntVariable);

    double db1RealVariable = S7.Net.Types.Double.FromByteArray(db1Bytes.Skip(4).Take(4).ToArray());
    Console.WriteLine("DB1.DBD4.0" + db1RealVariable);

    int db1DintVariable = S7.Net.Types.DInt.FromByteArray(db1Bytes.Skip(8).Take(4).ToArray());
    Console.WriteLine("DB1.DBD8.0" + db1DintVariable);

    uint db1DwordVariable = S7.Net.Types.DWord.FromByteArray(db1Bytes.Skip(12).Take(4).ToArray());
    Console.WriteLine("Db1.DBD12.0 " + db1DwordVariable);

    ushort db1WordVariable = S7.Net.Types.Word.FromByteArray(db1Bytes.Skip(16).Take(2).ToArray());
    Console.WriteLine("DB1.DBW14.0: " + db1WordVariable);


    //bool db1Bool1 = (bool)plc.Read("DB1.DBX0.0");
    //Console.WriteLine("DB1.DBX0.0: " + db1Bool1);

    //bool db1Bool2 = (bool)plc.Read("DB1.DBX0.1");
    //Console.WriteLine("DB1.DBX0.1: " + db1Bool2);

    //var db1IntVariable = ((ushort)plc.Read("DB1.DBW2.0")).ConvertToShort();
    //Console.WriteLine("DB1.DBW2.0: " + db1IntVariable);

    //var db1RealVariable = (uint)plc.Read("DB1.DBD4.0");
    //double db1RealVariable_Double;
    //db1RealVariable_Double = Convert.ToDouble(db1RealVariable);
    //Console.WriteLine("DB1.DBD4.0: " + db1RealVariable_Double);

    //var db1DintVariable = ((uint)plc.Read("DB1.DBD8.0")).ConvertToInt();
    //Console.WriteLine("DB1.DBD8.0: " + db1DintVariable);

    //var db1DwordVariable = ((uint)plc.Read("DB1.DBD12.0")).ConvertToInt();
    //Console.WriteLine("DB1.DBD12.0: " + db1DwordVariable);

    //var db1WordVariable = ((ushort)plc.Read("DB1.DBW16.0")).ConvertToShort();
    //Console.WriteLine("DB1.DBW16.0: " + db1WordVariable);

    //var db1T1Time = (uint)plc.Read("DB1.DBD26.0");
    //Console.WriteLine("DB1.DBD26.0: " + db1T1Time);

    Console.WriteLine("\n--- DB 5 ---\n");
    var db5 = new Db5();
    plc.ReadClass(db5, 5);

    Console.WriteLine("DB5.DBX0.0: " + db5.Bool1);
    Console.WriteLine("DB5.DBX0.1: " + db5.Bool2);
    Console.WriteLine("DB5.DBW2.0: " + db5.IntVariable);
    Console.WriteLine("DB5.DBD4.0: " + db5.RealVariable);
    Console.WriteLine("DB5.DBD8.0: " + db5.DintVariable);
    Console.WriteLine("DB5.DBD12.0: " + db5.DwordVariable);
    Console.WriteLine("DB5.DBW12.0: " + db5.WordVariable);



    Console.WriteLine("\n--- DB 2 ---\n");

    var db2 = new DB2();
    plc.ReadClass(db2, 2);

    
    double vOut = Convert.ToDouble(db2.RealVariable);

    Console.WriteLine("DB2.DBX0.0: " + db2.Bool1);
    Console.WriteLine("DB2.DBX0.1: " + db2.Bool2);
    Console.WriteLine("DB2.DBW2.0: " + db2.IntVariable);
    Console.WriteLine("DB2.DBD4.0: " + db2.RealVariable);
    Console.WriteLine("DB2.DBD8.0: " + db2.DintVariable);
    Console.WriteLine("DB2.DBD12.0: " + db2.DwordVariable);
    Console.WriteLine("DB2.DBW12.0: " + db2.WordVariable);



    //var db2Bytes = plc.ReadBytes(DataType.DataBlock, 2, 0, 22);
    //Console.WriteLine("ReadBytes DB2 Bytes" + db2Bytes);

    //bool db2Bool1 = db2Bytes[0].SelectBit(0);
    //Console.WriteLine("DB2.DBX0.0: " + db2Bool1);

    //bool db2Bool2 = db2Bytes[0].SelectBit(1);
    //Console.WriteLine("DB2.DBX0.1: " + db2Bool2);

    //short db2IntVariable = S7.Net.Types.Int.FromByteArray(db2Bytes.Skip(2).Take(2).ToArray());
    //Console.WriteLine("DB2.DBW2.0" + db2IntVariable);

    //double db2RealVariable = S7.Net.Types.Double.FromByteArray(db2Bytes.Skip(4).Take(4).ToArray());
    //Console.WriteLine("DB2.DBD4.0" + db2RealVariable);

    //int db2DintVariable = S7.Net.Types.DInt.FromByteArray(db2Bytes.Skip(8).Take(4).ToArray());
    //Console.WriteLine("DB2.DBD8.0" + db2DintVariable);

    //uint db2DwordVariable = S7.Net.Types.DWord.FromByteArray(db2Bytes.Skip(12).Take(4).ToArray());
    //Console.WriteLine("Db2.DBD12.0 " + db2DwordVariable);

    //ushort db2WordVariable = S7.Net.Types.Word.FromByteArray(db2Bytes.Skip(16).Take(2).ToArray());
    //Console.WriteLine("DB2.DBW16.0: " + db2WordVariable);

    //double db2RealVariable2 = S7.Net.Types.Double.FromByteArray(db2Bytes.Skip(18).Take(4).ToArray());
    //Console.WriteLine("DB2.DBD18.0" + db2RealVariable2);

    //var db2 = new DB2();
    //plc.ReadClass(db2, 2);


    Console.WriteLine("\n--- DB 3 ---\n");

    var db3Bytes = plc.ReadBytes(DataType.DataBlock, 3, 0, 22);
    Console.WriteLine("ReadBytes DB1 Bytes" + db1Bytes);

    bool db3Bool1 = db3Bytes[0].SelectBit(0);
    Console.WriteLine("DB3.DBX0.0: " + db3Bool1);

    bool db3Bool2 = db3Bytes[0].SelectBit(1);
    Console.WriteLine("DB3.DBX0.1: " + db3Bool2);

    short db3IntVariable = S7.Net.Types.Int.FromByteArray(db3Bytes.Skip(2).Take(2).ToArray());
    Console.WriteLine("DB3.DBW2.0" + db3IntVariable);

    double db3RealVariable = S7.Net.Types.Double.FromByteArray(db3Bytes.Skip(4).Take(4).ToArray());
    Console.WriteLine("DB3.DBD4.0" + db3RealVariable);

    int db3DintVariable = S7.Net.Types.DInt.FromByteArray(db3Bytes.Skip(8).Take(4).ToArray());
    Console.WriteLine("DB3.DBD8.0" + db3DintVariable);

    uint db3DwordVariable = S7.Net.Types.DWord.FromByteArray(db3Bytes.Skip(12).Take(4).ToArray());
    Console.WriteLine("Db3.DBD12.0 " + db3DwordVariable);

    ushort db3WordVariable = S7.Net.Types.Word.FromByteArray(db3Bytes.Skip(16).Take(2).ToArray());
    Console.WriteLine("DB3.DBW16.0: " + db3WordVariable);

    double db3RealVariable2 = S7.Net.Types.Double.FromByteArray(db3Bytes.Skip(18).Take(4).ToArray());
    Console.WriteLine("DB3.DBD18.0" + db3RealVariable2);

    //var db3Bool1 = (bool)plc.Read("DB3.DBX0.0");
    //Console.WriteLine("DB3.DBX0.0: " + db3Bool1);

    //var db3Bool2 = (bool)plc.Read("DB3.DBX0.1");
    //Console.WriteLine("DB3.DBX0.1: " + db3Bool2);

    //var db3IntVariable = ((ushort)plc.Read("DB3.DBW2.0")).ConvertToShort();
    //Console.WriteLine("DB3.DBW2.0: " + db3IntVariable);

    //var db3RealVariable = (uint)plc.Read("DB3.DBD4.0");
    //Console.WriteLine("DB3.DBD4.0: " + db3RealVariable);

    //var db3DintVariable = ((uint)plc.Read("DB3.DBD8.0")).ConvertToInt();
    //Console.WriteLine("DB3.DBD8.0: " + db3DintVariable);

    //var db3DwordVariable = ((uint)plc.Read("DB3.DBD12.0")).ConvertToInt();
    //Console.WriteLine("DB3.DBD12.0: " + db3DwordVariable);

    //var db3WordVariable = ((ushort)plc.Read("DB3.DBW16.0")).ConvertToShort();
    //Console.WriteLine("DB3.DBW16.0: " + db3WordVariable);
}

Console.ReadKey();
