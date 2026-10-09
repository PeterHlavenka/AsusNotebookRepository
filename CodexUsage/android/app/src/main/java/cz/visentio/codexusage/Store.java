package cz.visentio.codexusage;

import android.content.Context;
import android.security.keystore.KeyGenParameterSpec;
import android.security.keystore.KeyProperties;
import android.util.Base64;
import org.json.*;
import java.security.KeyStore;
import javax.crypto.*;
import javax.crypto.spec.GCMParameterSpec;

/** Only encrypted credentials are stored; Android backups are disabled. */
final class Store {
    private static final String KEY = "codex_usage_accounts_v1";
    private static javax.crypto.SecretKey key() throws Exception {
        KeyStore store = KeyStore.getInstance("AndroidKeyStore"); store.load(null);
        if (!store.containsAlias(KEY)) {
            KeyGenerator generator = KeyGenerator.getInstance("AES", "AndroidKeyStore");
            generator.init(new KeyGenParameterSpec.Builder(KEY, KeyProperties.PURPOSE_ENCRYPT | KeyProperties.PURPOSE_DECRYPT)
                .setBlockModes(KeyProperties.BLOCK_MODE_GCM).setEncryptionPaddings(KeyProperties.ENCRYPTION_PADDING_NONE).build());
            generator.generateKey();
        }
        return (javax.crypto.SecretKey)store.getKey(KEY, null);
    }
    static synchronized JSONArray read(Context c) throws Exception {
        String value = c.getSharedPreferences("secure", 0).getString("accounts", null);
        if (value == null) return new JSONArray();
        String[] parts = value.split(":");
        Cipher cipher = Cipher.getInstance("AES/GCM/NoPadding");
        cipher.init(Cipher.DECRYPT_MODE, key(), new GCMParameterSpec(128, Base64.decode(parts[0], Base64.NO_WRAP)));
        return new JSONArray(new String(cipher.doFinal(Base64.decode(parts[1], Base64.NO_WRAP)), java.nio.charset.StandardCharsets.UTF_8));
    }
    private static void write(Context c, JSONArray accounts) throws Exception {
        Cipher cipher = Cipher.getInstance("AES/GCM/NoPadding"); cipher.init(Cipher.ENCRYPT_MODE, key());
        String value = Base64.encodeToString(cipher.getIV(), Base64.NO_WRAP) + ":" +
            Base64.encodeToString(cipher.doFinal(accounts.toString().getBytes(java.nio.charset.StandardCharsets.UTF_8)), Base64.NO_WRAP);
        if (!c.getSharedPreferences("secure", 0).edit().putString("accounts", value).commit())
            throw new Exception("Účet se nepodařilo uložit");
    }
    static synchronized void add(Context c, JSONObject account) throws Exception {
        JSONArray accounts = read(c);
        if (accounts.length() >= 2) throw new Exception("Nejvýše dva účty; nejdřív jeden odeber");
        for (int i=0; i<accounts.length(); i++) {
            JSONObject old = accounts.getJSONObject(i);
            if (old.optString("accountId").equals(account.optString("accountId")) && old.optString("email").equals(account.optString("email")))
                throw new Exception("Tento účet už je přidaný");
        }
        accounts.put(account); write(c, accounts);
    }
    static synchronized void replace(Context c, JSONObject account) throws Exception {
        JSONArray accounts = read(c);
        for (int i=0; i<accounts.length(); i++) if (accounts.getJSONObject(i).getString("id").equals(account.getString("id"))) {
            accounts.put(i, account); write(c, accounts); return;
        }
    }
    static synchronized void remove(Context c, String id) throws Exception {
        JSONArray accounts=read(c), result=new JSONArray();
        for (int i=0;i<accounts.length();i++) if (!accounts.getJSONObject(i).getString("id").equals(id)) result.put(accounts.get(i));
        write(c,result);
    }
    static void reset(Context c) { c.getSharedPreferences("secure",0).edit().clear().apply(); }
    static boolean live(Context c) { return c.getSharedPreferences("settings",0).getBoolean("live",false); }
    static void live(Context c, boolean value) { c.getSharedPreferences("settings",0).edit().putBoolean("live",value).apply(); }
}
