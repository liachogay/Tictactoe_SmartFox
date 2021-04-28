package TicTacToe;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.exceptions.SFSLoginException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

public class LeaveRoomEventHandler extends BaseServerEventHandler {

	@Override
	public void handleServerEvent(ISFSEvent arg0) throws SFSException {
		// TODO Auto-generated method stub
		User username = (User) arg0.getParameter(SFSEventParam.USER);
		Room room = (Room) arg0.getParameter(SFSEventParam.ROOM);
		if (room.getGroupId().equals("games")) {
			try {
				Connection conn = getParentExtension().getParentZone().getDBManager().getConnection();
				PreparedStatement sql = conn.prepareStatement("exec GetMoneyByName ?");
				sql.setString(1, username.getName().trim());
				ResultSet rs = sql.executeQuery();

				ISFSArray row = SFSArray.newFromResultSet(rs);
				if (row.size() > 0) {
					long moneyHave = row.getSFSObject(0).getLong("MoneyHave");
					if (moneyHave > 100) {
						moneyHave -= 100;
					} else {
						moneyHave = 0;
					}
					sql = conn.prepareStatement("EXEC SetMoneyByID ?, ?");
					sql.setInt(1, row.getSFSObject(0).getInt("ID"));
					sql.setLong(2, moneyHave);
					sql.executeUpdate();
					trace("money have: " + moneyHave);
				} else {
					throw new SFSLoginException(
							"The account " + username.getName() + " is not sign up in the database");
				}

			} catch (SQLException e) {
				e.printStackTrace();
			}
		}
	}

}
