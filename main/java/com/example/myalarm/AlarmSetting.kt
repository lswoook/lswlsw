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
import kotlinx.android.synthetic.main.alarm_setting.*
import java.util.*

class AlarmSetting:AppCompatActivity() {
    private var alarmList = listOf<Alarm>()
    private var alarmDb : AlarmDB? = null
    lateinit var  am: AlarmManager
    lateinit var pi : PendingIntent
    var Anote : String = ""

    @TargetApi(Build.VERSION_CODES.M)
    @RequiresApi(Build.VERSION_CODES.KITKAT)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.alarm_setting)
        alarmDb = AlarmDB.getInstance(this)
        var DBid : Long? = 0
        //알람 시스템 서비스 셋팅
        am = getSystemService(Context.ALARM_SERVICE) as AlarmManager
        var calendar:Calendar = Calendar.getInstance()
        var myIntent: Intent = Intent(this,AlarmReceiver::class.java)

        //설정완료 버튼 누름
        btnFinish.setOnClickListener {

            //켜져있는 알람 피커에서 정보를 받아온다
            val timePicker = findViewById<TimePicker>(R.id.timePicker)
            val hour = timePicker.hour
            val minute = timePicker.minute
            val Str = hour.toString() + ":"+minute.toString()

            calendar.set(Calendar.HOUR_OF_DAY,hour)
            calendar.set(Calendar.MINUTE,minute)
            calendar.set(Calendar.SECOND,0)
            calendar.set(Calendar.MILLISECOND,0)
            if(calendar.timeInMillis < System.currentTimeMillis()){
                calendar.add(Calendar.DAY_OF_YEAR,1)
            }

            //데이터 베이스에 알람 정보를 추가
            val addRunnable = Runnable {
                val newAlarm = Alarm()
                newAlarm.alarmTime = Str
                newAlarm.alarmNote = Anote
                var currentDBid = newAlarm.id
                DBid = currentDBid
                alarmDb?.alarmDao()?.insert(newAlarm)
            }
            val addThread = Thread(addRunnable)
            addThread.start()

            /*
            // 알람메니저로 데이터 전송
            var hr_str : String = hour.toString()
            var min_str : String = minute.toString()
            if (hour > 12)
            {
                hr_str = (hour-12).toString()
            }
            if (minute < 10)
            {
                min_str = "0$minute"
            }
            set_alarm_text("Alarm set to : $hr_str : $min_str")
            myIntent.putExtra("extra" , "on")
            pi = PendingIntent.getBroadcast(this, DBid!!.toInt(),myIntent,PendingIntent.FLAG_UPDATE_CURRENT)
            //설정하는 시간이 현재보다 과거인 경우 하루를 더한다
            if(calendar.timeInMillis < System.currentTimeMillis()){
                calendar.add(Calendar.DAY_OF_YEAR,1)
            }
            am.setExact(AlarmManager.RTC_WAKEUP,calendar.timeInMillis,pi)
*/


            //메인화면 시작
            val intent = Intent(this, MainActivity::class.java)
            startActivity(intent)
            finish()
        }
    }
    private fun set_alarm_text(s: String) {
        Anote = s
    }

    override fun onDestroy() {
        AlarmDB.destroyInstance()
        super.onDestroy()
    }
}