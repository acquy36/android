package md50ee9bc21019331289bb4e0313382fea2;


public class DataBase
	extends android.database.sqlite.SQLiteOpenHelper
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/database/sqlite/SQLiteDatabase;)V:GetOnCreate_Landroid_database_sqlite_SQLiteDatabase_Handler\n" +
			"n_onUpgrade:(Landroid/database/sqlite/SQLiteDatabase;II)V:GetOnUpgrade_Landroid_database_sqlite_SQLiteDatabase_IIHandler\n" +
			"";
		mono.android.Runtime.register ("QLSX_2.Resources.Data.DataBase, QLSX_2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DataBase.class, __md_methods);
	}


	public DataBase (android.content.Context p0, java.lang.String p1, android.database.sqlite.SQLiteDatabase.CursorFactory p2, int p3) throws java.lang.Throwable
	{
		super (p0, p1, p2, p3);
		if (getClass () == DataBase.class)
			mono.android.TypeManager.Activate ("QLSX_2.Resources.Data.DataBase, QLSX_2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Android.Database.Sqlite.SQLiteDatabase+ICursorFactory, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public DataBase (android.content.Context p0, java.lang.String p1, android.database.sqlite.SQLiteDatabase.CursorFactory p2, int p3, android.database.DatabaseErrorHandler p4) throws java.lang.Throwable
	{
		super (p0, p1, p2, p3, p4);
		if (getClass () == DataBase.class)
			mono.android.TypeManager.Activate ("QLSX_2.Resources.Data.DataBase, QLSX_2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Android.Database.Sqlite.SQLiteDatabase+ICursorFactory, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Android.Database.IDatabaseErrorHandler, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1, p2, p3, p4 });
	}


	public void onCreate (android.database.sqlite.SQLiteDatabase p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.database.sqlite.SQLiteDatabase p0);


	public void onUpgrade (android.database.sqlite.SQLiteDatabase p0, int p1, int p2)
	{
		n_onUpgrade (p0, p1, p2);
	}

	private native void n_onUpgrade (android.database.sqlite.SQLiteDatabase p0, int p1, int p2);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
