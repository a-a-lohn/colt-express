package main;

import com.smartfoxserver.v2.extensions.SFSExtension;


public class ColtExtension extends SFSExtension
{
    @Override
    public void init()
    {
    	addRequestHandler("gm", ColtMultiHandler.class);
    }
 
    @Override
    public void destroy()
    {
        super.destroy();
        trace("Destroy is called!");
    }
 
}
