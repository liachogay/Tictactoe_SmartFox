package TicTacToe;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class GetMoneyHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		DBExtension parent = (DBExtension) getParentExtension();
		ISFSObject ret = new SFSObject();
		try {
			Connection conn = getParentExtension().getParentZone().getDBManager().getConnection();

			PreparedStatement sql = conn.prepareStatement("exec GetMoneyByName ?");
			sql.setString(1, arg0.getName());

			ResultSet result = sql.executeQuery();

			ISFSArray row = SFSArray.newFromResultSet(result);
			if (row.size() > 0) {
				ret.putBool("success", true);
				ret.putLong("money", row.getSFSObject(0).getLong("MoneyHave"));
			} else {
				ret.putBool("success", false);
			}
			conn.close();

		} catch (SQLException e) {
			ret.putBool("success", false);
			e.printStackTrace();
		}
		parent.send("getMoney", ret, arg0);
	}

}
