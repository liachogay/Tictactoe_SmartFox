package TicTacToe;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.RoomVariable;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class GetSpotEmptyHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		TicTacToeExtension parent = (TicTacToeExtension) getParentExtension();

		RoomVariable board = parent.getParentRoom().getVariable("board");
		ISFSObject SFSBoard = board.getSFSObjectValue();
		ISFSObject boardEmpty = new SFSObject();

		List<Integer> intList = (List<Integer>) SFSBoard.getIntArray("board");

		int sizeOfBoard = intList.size();
		Collection<Boolean> boardSpotEmpty = new ArrayList<Boolean>();
		for (int i = 0; i < sizeOfBoard; i++) {
			if (intList.get(i) != 0) {
				boardSpotEmpty.add(false);
			} else {
				boardSpotEmpty.add(true);
			}
		}

		boardEmpty.putBoolArray("boardEmpty", boardSpotEmpty);

		parent.send("getSpotEmpty", boardEmpty, getParentExtension().getParentRoom().getPlayersList());
	}

}
