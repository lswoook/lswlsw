package com.example.myalarm

import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import androidx.core.content.ContextCompat.startForegroundService
import android.os.Build



class AlarmReceiver : BroadcastReceiver() {
    override fun onReceive(context: Context?, intent: Intent?) {

        var getResult:String = intent!!.getStringExtra("extra")

        var service_intent :  Intent = Intent(context,RingtoneService::class.java )
        service_intent.putExtra("extra",getResult)

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
                context!!.startForegroundService(service_intent)
        } else {
            context!!.startService(service_intent)
        }
    }
}