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

public class SignUpHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		DBExtension parent = (DBExtension) getParentExtension();

		ISFSObject ret = new SFSObject();

		String username = (String) arg1.getUtfString("username");
		String password = (String) arg1.getUtfString("password");

		trace("Called signup client request");

		try {
			Connection conn = getParentExtension().getParentZone().getDBManager().getConnection();

			PreparedStatement sql = conn.prepareStatement("exec CheckAcc ?");
			sql.setString(1, username);

			ResultSet result = sql.executeQuery();

			ISFSArray row = SFSArray.newFromResultSet(result);

			if (row.size() > 0) {
				ret.putBool("success", false);
				ret.putUtfString("error", "Username is contained");
			} else {
				trace("Everything is okay");
				ret.putBool("success", true);
				sql = conn.prepareStatement("exec SignUp ? , ?");
				sql.setString(1, username);
				sql.setString(2, password);
				sql.executeUpdate();
			}
			conn.close();

		} catch (SQLException e) {
			ret.putBool("success", false);
			e.printStackTrace();
		}
		parent.send("signup", ret, arg0);
	}
}
