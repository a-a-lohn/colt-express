package main;

import com.smartfoxserver.v2.extensions.SFSExtension;

public class ColtExtension extends SFSExtension
{
    @Override
    public void init()
    {
    	/*
    	 * Add request handlers here for info sent to server from client
    	 * 
    	 * Make GameManager a @MultiHandler Extension that delegates incoming client requests to different methods,
    	 * and then those methods send back responses directly to user(s)
    	 */
        addRequestHandler("gm", GameManager.class); // requests on the client must of the form "gm.method_name"
    }
 
    @Override
    public void destroy()
    {
        // Always make sure to invoke the parent class first
        super.destroy();
        trace("Destroy is called!");
    }
 
    /*public class ZoneEventHandler extends BaseServerEventHandler
    {
        @Override
        public void handleServerEvent(ISFSEvent event) throws SFSException
        {
            User user = (User) event.getParameter(SFSEventParam.USER);
            trace("Welcome new user: " + user.getName());
        }
    }*/
}
