package TicTacToe;

import java.util.Random;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class TestHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		Random rand = new Random();
		ISFSObject ret = new SFSObject();
		ret.putInt("test", rand.nextInt());
		getParentExtension().send("test", ret, arg0);
	}

}
