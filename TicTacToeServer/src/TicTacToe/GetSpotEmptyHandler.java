package TicTacToe;

import java.util.Arrays;
import java.util.Collection;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class GetSpotEmptyHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		TicTacToeExtension parent = (TicTacToeExtension) getParentExtension();

		ISFSObject ret = new SFSObject();
		Collection<Boolean> collection = Arrays.asList(BoardGame.Instance().getBoardDataEmpty());
		ret.putBoolArray("boardEmpty", collection);
		
		parent.send("getSpotEmpty", ret, getParentExtension().getParentRoom().getPlayersList());
	}

}
