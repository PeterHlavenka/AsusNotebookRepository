package cz.visentio.codexusage;

import org.json.*;
import org.junit.Test;
import static org.junit.Assert.*;

public class WidgetAccountsTest {
    private JSONArray accounts()throws Exception {
        return new JSONArray().put(new JSONObject().put("id","personal-profile").put("kind","personal"))
            .put(new JSONObject().put("id","work-profile").put("kind","work"));
    }
    @Test public void singleWidgetUsesTheChosenIdentity()throws Exception {
        JSONArray source=accounts();
        assertEquals("work-profile",WidgetAccounts.select(source,true,"work-profile").getJSONObject(0).getString("id"));
        assertEquals("personal-profile",WidgetAccounts.select(source,true,"personal-profile").getJSONObject(0).getString("id"));
    }
    @Test public void removedAccountCannotFallBackToAnotherAccount()throws Exception {
        assertEquals(0,WidgetAccounts.select(accounts(),true,"deleted-profile").length());
        assertEquals(0,WidgetAccounts.select(accounts(),true,null).length());
    }
    @Test public void twoAccountWidgetKeepsWorkFirstWithoutMutatingConfiguration()throws Exception {
        JSONArray source=accounts();JSONArray selected=WidgetAccounts.select(source,false,null);
        assertEquals(2,selected.length());assertEquals("work-profile",selected.getJSONObject(0).getString("id"));
        assertEquals("personal-profile",source.getJSONObject(0).getString("id"));
    }
    @Test public void singleAvailableAccountStillAppearsInTheCombinedWidget()throws Exception {
        JSONArray source=new JSONArray().put(accounts().getJSONObject(0));
        assertEquals("personal-profile",WidgetAccounts.select(source,false,null).getJSONObject(0).getString("id"));
    }
}
