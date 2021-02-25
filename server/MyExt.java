package main;

import com.smartfoxserver.v2.extensions.SFSExtension;

public class MyExt extends SFSExtension
{
    @Override
    public void init()
    {
        addRequestHandler("math", MathHandler.class);
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
