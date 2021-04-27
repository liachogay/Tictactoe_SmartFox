package TicTacToe;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.RoomVariable;
import com.smartfoxserver.v2.entities.variables.SFSRoomVariable;
import com.smartfoxserver.v2.exceptions.SFSVariableException;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class ReadyHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		TicTacToeExtension parent = (TicTacToeExtension) getParentExtension();
		Room room = parent.getParentRoom();
		int playerInRoom = room.getPlayersList().size();

		ISFSArray ISSArrayID = new SFSArray();
		ISFSArray ISSArrayName = new SFSArray();

		trace("client request ready");

		if (room.getVariable("initGame") == null) {
			ISSArrayID.addInt(arg0.getId());
			ISSArrayName.addUtfString(arg0.getName());
			List<RoomVariable> listVar = new ArrayList<RoomVariable>();
			RoomVariable turn = new SFSRoomVariable("turn", arg0.getId());
			RoomVariable playerID = new SFSRoomVariable("playerID", ISSArrayID);
			RoomVariable playerName = new SFSRoomVariable("playerName", ISSArrayName);
			RoomVariable initgame = new SFSRoomVariable("initGame", true);
			listVar.add(turn);
			listVar.add(playerID);
			listVar.add(playerName);
			listVar.add(initgame);
			room.setVariables(listVar);
		} else {
			ISSArrayID = room.getVariable("playerID").getSFSArrayValue();
			ISSArrayName = room.getVariable("playerName").getSFSArrayValue();
			trace("ID: " +arg0.getId() + " name: " + arg0.getName());
			ISSArrayID.addInt(arg0.getId());
			ISSArrayName.addUtfString(arg0.getName());
			List<RoomVariable> listVar = new ArrayList<RoomVariable>();
			RoomVariable playerID = new SFSRoomVariable("playerID", ISSArrayID);
			RoomVariable playerName = new SFSRoomVariable("playerName", ISSArrayName);
			listVar.add(playerID);
			listVar.add(playerName);
			room.setVariables(listVar);
		}

		trace("Player in room: " + playerInRoom);

		if (room.getMaxUsers() == playerInRoom) {
			Collection<Integer> cInteger = new ArrayList<Integer>();
			cInteger.add(Tile.Empty.getValue());
			cInteger.add(Tile.Empty.getValue());
			cInteger.add(Tile.Empty.getValue());
			cInteger.add(Tile.Empty.getValue());
			cInteger.add(Tile.Empty.getValue());
			cInteger.add(Tile.Empty.getValue());
			cInteger.add(Tile.Empty.getValue());
			cInteger.add(Tile.Empty.getValue());
			cInteger.add(Tile.Empty.getValue());

			ISFSObject SFSBoard = new SFSObject();
			SFSBoard.putIntArray("board", cInteger);

			RoomVariable board = new SFSRoomVariable("board", SFSBoard);
			try {
				room.setVariable(board);
			} catch (SFSVariableException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}

			ISFSObject sfsx = new SFSObject();
			sfsx.putInt("turn", room.getVariable("turn").getIntValue());
			sfsx.putInt("idPlayer1", ISSArrayID.getInt(0));
			sfsx.putInt("idPlayer2", ISSArrayID.getInt(1));
			sfsx.putUtfString("namePlayer1", ISSArrayName.getUtfString(0));
			sfsx.putUtfString("namePlayer2", ISSArrayName.getUtfString(1));
			parent.send("start", sfsx, parent.getParentRoom().getPlayersList());
		}
	}

}
