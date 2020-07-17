package com.example.myalarm

import android.annotation.TargetApi
import android.app.AlarmManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.os.Build
import android.os.Bundle
import android.widget.TimePicker
import android.widget.Toast
import androidx.annotation.RequiresApi
import androidx.appcompat.app.AppCompatActivity
import kotlinx.android.synthetic.main.alarm_layout.*
import java.util.*

class AlarmEdit: AppCompatActivity() {
    private var alarmList = listOf<Alarm>()
    private var alarmDb : AlarmDB? = null
    lateinit var  am: AlarmManager
    lateinit var pi : PendingIntent
    var Anote : String = ""

    @TargetApi(Build.VERSION_CODES.M)
    @RequiresApi(Build.VERSION_CODES.KITKAT)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.alarm_layout)
        alarmDb = AlarmDB.getInstance(this)
        am = getSystemService(Context.ALARM_SERVICE) as AlarmManager

        //메인에서 클릭된 데이터의 아이디 값을 받아왔다
        var currentDBid = intent.getStringExtra("DBid")
        var calendar:Calendar = Calendar.getInstance()

        //메인에서 클릭된 데이터의 시간값, 노트값을 받아옸다... < 미구현

        //시간값

        //노트값

        //설정된 알람 시간

        //가져온 아이디의 해당 되는 알람 시스템 서비스 셋팅
        am = getSystemService(Context.ALARM_SERVICE) as AlarmManager
        var myIntent: Intent = Intent(this,AlarmReceiver::class.java)

        //수정하기
        btnFinish_edit.setOnClickListener{
            val timePicker = findViewById<TimePicker>(R.id.timePicker_edit)
            val hour = timePicker.hour
            val minute = timePicker.minute
            val Str = hour.toString() + ":"+minute.toString()
            calendar.set(Calendar.HOUR_OF_DAY,hour)
            calendar.set(Calendar.MINUTE,minute)
            calendar.set(Calendar.SECOND,0)
            calendar.set(Calendar.MILLISECOND,0)

            val addRunnable = Runnable {
                val newAlarm = Alarm()
                newAlarm.id = currentDBid.toLong()
                newAlarm.alarmNote = "Hi"
                newAlarm.alarmTime = "$hour" +":$minute"
                alarmDb?.alarmDao()?.update(newAlarm)
            }
            val addThread = Thread(addRunnable)
            addThread.start()

            //메인화면을 엽니다
            val intent = Intent(this, MainActivity::class.java)
            startActivity(intent)
            finish()
        }
        //지우기
        btnDel.setOnClickListener {
            //데이터베이스에서 삭제
            val addRunnable = Runnable {
                val newAlarm = Alarm()
                newAlarm.id = currentDBid.toLong()
                alarmDb?.alarmDao()?.delete(newAlarm)
            }
            val addThread = Thread(addRunnable)
            addThread.start()

            //알람을 끔
            myIntent.putExtra("extra" , "off")
            pi = PendingIntent.getBroadcast(this,currentDBid.toInt(),myIntent,PendingIntent.FLAG_UPDATE_CURRENT)
            am.cancel(pi)
            sendBroadcast(myIntent)

            //메인화면을 엽니다
            val intent = Intent(this, MainActivity::class.java)
            startActivity(intent)
            finish()
        }

    }
    override fun onDestroy() {
        AlarmDB.destroyInstance()
        super.onDestroy()
    }
}
