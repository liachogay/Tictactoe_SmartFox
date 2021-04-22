package TicTacToe;

import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class ReadyHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		TicTacToeExtension parent = (TicTacToeExtension) getParentExtension();
		Room room = parent.getParentRoom();
		int playerInRoom = room.getPlayersList().size();

		trace("Max player in room: " + playerInRoom);

		for (int i = 0; i < playerInRoom; i++) {
			trace(i + " " + PlayerTurnManager.Instance().getPlayers(i));
			if (PlayerTurnManager.Instance().getPlayers(i) == 0) {
				trace("Set data user: " + arg0.getId() + " " + arg0.getName());
				PlayerTurnManager.Instance().setPlayers(i, arg0.getId());
				PlayerTurnManager.Instance().setNamePlayers(i, arg0.getName());
				trace("player: " + PlayerTurnManager.Instance().getPlayers(i));
				trace("player name: " + PlayerTurnManager.Instance().getNamePlayers(i));
				break;
			}
		}
		ISFSObject sfs = new SFSObject();
		if (2 == playerInRoom) {
			PlayerTurnManager.Instance().setTurn(PlayerTurnManager.Instance().getPlayers(0));
			// first people first turn
			sfs.putInt("turn", PlayerTurnManager.Instance().getPlayers(0));
			sfs.putInt("idPlayer1", PlayerTurnManager.Instance().getPlayers(0));
			sfs.putInt("idPlayer2", PlayerTurnManager.Instance().getPlayers(1));
			sfs.putUtfString("namePlayer1", PlayerTurnManager.Instance().getNamePlayers(0));
			sfs.putUtfString("namePlayer2", PlayerTurnManager.Instance().getNamePlayers(1));

			parent.send("start", sfs, parent.getParentRoom().getPlayersList());
		} else {
			trace("Have 1 people join room: " + parent.getParentRoom().getName());
		}
	}

}
