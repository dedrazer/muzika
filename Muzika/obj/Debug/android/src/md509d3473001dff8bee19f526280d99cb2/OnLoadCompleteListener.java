package md509d3473001dff8bee19f526280d99cb2;


public class OnLoadCompleteListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.media.SoundPool.OnLoadCompleteListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onLoadComplete:(Landroid/media/SoundPool;II)V:GetOnLoadComplete_Landroid_media_SoundPool_IIHandler:Android.Media.SoundPool/IOnLoadCompleteListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("MuzikaClasses.OnLoadCompleteListener, View, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", OnLoadCompleteListener.class, __md_methods);
	}


	public OnLoadCompleteListener ()
	{
		super ();
		if (getClass () == OnLoadCompleteListener.class)
			mono.android.TypeManager.Activate ("MuzikaClasses.OnLoadCompleteListener, View, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onLoadComplete (android.media.SoundPool p0, int p1, int p2)
	{
		n_onLoadComplete (p0, p1, p2);
	}

	private native void n_onLoadComplete (android.media.SoundPool p0, int p1, int p2);

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
