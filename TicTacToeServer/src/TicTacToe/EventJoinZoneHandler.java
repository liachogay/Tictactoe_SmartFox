package TicTacToe;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

public class EventJoinZoneHandler extends BaseServerEventHandler {

	@Override
	public void handleServerEvent(ISFSEvent arg0) throws SFSException {
		// TODO Auto-generated method stub
		User user = (User) arg0.getParameter(SFSEventParam.USER);
		trace("Welcome new user: " + user.getName());
	}

}
