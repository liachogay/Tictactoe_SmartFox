package TicTacToe;

import java.util.Arrays;
import java.util.Collection;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class MoveHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		TicTacToeExtension parent = (TicTacToeExtension) getParentExtension();
		
		if (arg0.getId() == PlayerTurnManager.Instance().getPlayers(0)) {
			arg1.putUtfString("text", "X");
		} else {
			arg1.putUtfString("text", "O");
		}
		arg1.putInt("sender", arg0.getId());
		int x = arg1.getInt("x");
		int y = arg1.getInt("y");

		BoardGame.Instance().setBoardData(x, y, Tile.Tic);
		parent.send("move", arg1, parent.getParentRoom().getPlayersList());

		PlayerTurnManager.Instance().nextTurn();
		
		User turnUser = parent.getParentRoom().getUserByPlayerId(PlayerTurnManager.Instance().getTurn());

		Collection<Boolean> collection = Arrays.asList(BoardGame.Instance().getBoardDataEmpty());

		ISFSObject nextTurn = new SFSObject();
		nextTurn.putBoolArray("boardEmpty", collection);
				
		trace(turnUser.getName());
		
		parent.send("getSpotEmpty", nextTurn, turnUser);
	}
}
