package com.example.myalarm

import android.app.AlarmManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.os.Bundle
import android.os.PersistableBundle
import androidx.appcompat.app.AppCompatActivity
import kotlinx.android.synthetic.main.alarm_screen.*
import java.util.*

class AlarmScreen :AppCompatActivity(){

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.alarm_screen)


        //메인에서 클릭된 데이터의 아이디 값을 받아왔다
        var AlarmId = intent.getStringExtra("AlarmId")
        var myIntent: Intent = Intent(this,AlarmReceiver::class.java)
        lateinit var  am: AlarmManager
        lateinit var pi : PendingIntent
        am = getSystemService(Context.ALARM_SERVICE) as AlarmManager


        btn_stopAlarm.setOnClickListener {
            myIntent.putExtra("extra" , "off")
            pi = PendingIntent.getBroadcast(this,AlarmId.toInt(),myIntent, PendingIntent.FLAG_UPDATE_CURRENT)
            am.cancel(pi)
            sendBroadcast(myIntent)
        }


    }

    override fun onDestroy() {
        super.onDestroy()
    }
}