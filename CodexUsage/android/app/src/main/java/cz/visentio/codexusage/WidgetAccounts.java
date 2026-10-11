package cz.visentio.codexusage;

import org.json.*;

final class WidgetAccounts {
    static JSONArray select(JSONArray accounts,boolean single,String selectedId) throws JSONException {
        JSONArray result=new JSONArray();
        if(single) {
            for(int i=0;i<accounts.length();i++) {
                JSONObject account=accounts.getJSONObject(i);
                if(account.optString("id").equals(selectedId)) { result.put(account);break; }
            }
            // A removed account never silently switches a widget to another identity.
            return result;
        }
        for(int i=0;i<accounts.length();i++)result.put(accounts.getJSONObject(i));
        if(result.length()==2&&result.getJSONObject(0).optString("kind").equals("personal")&&
            !result.getJSONObject(1).optString("kind").equals("personal")) {
            JSONObject first=result.getJSONObject(0);
            result.put(0,result.getJSONObject(1));result.put(1,first);
        }
        return result;
    }
}
