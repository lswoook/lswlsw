package com.example.myalarm

import android.app.*
import android.content.Context
import android.content.Intent
import android.graphics.Color
import android.media.Ringtone
import android.media.RingtoneManager
import android.net.Uri
import android.os.Build
import android.os.IBinder
import androidx.core.app.NotificationCompat
import androidx.core.app.NotificationManagerCompat

class RingtoneService : Service() {
    companion object{
        lateinit var  r: Ringtone
    }
    override fun onBind(intent: Intent?): IBinder? {
        return null
    }
    var id:Int = 0
    var isRunning:Boolean = false
    var AlarmId:Int = 0

    override fun onStartCommand(intent: Intent?, flags: Int, startId: Int): Int {
        var state:String = intent!!.getStringExtra("extra")
        if(intent.hasExtra("AlarmId")){
            AlarmId = intent!!.getStringExtra("AlarmId").toInt()
        }

        assert(state!=null)
        when(state)
        {
            "on" ->id = 1
            "off"->id = 0
        }
        if(!this.isRunning &&id == 1)
        {
            playAlarm()
            this.isRunning = true
            this.id = 0
            fireNotification()

        }
        else if (this.isRunning && id ==0)
        {
            r.stop()
            this.isRunning = false
            this.id = 0

        }
        else if (!this.isRunning && id ==0)
        {
            this.isRunning = false
            this.id = 0
        }
        else if (this.isRunning && id ==1)
        {
            this.isRunning = true
            this.id = 1
        }
        else
        {

        }
        return START_NOT_STICKY
    }
    private fun fireNotification () {
        var notificationManager: NotificationManager
        var notificationChannel: NotificationChannel
        var builder:Notification.Builder
        val channelId= "package com.example.myalarm"
        var description="My Notification"
        notificationManager = getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
        val intent = Intent(applicationContext,AlarmScreen::class.java)

        val pendingIntent = PendingIntent.getActivity(this,0,intent,PendingIntent.FLAG_UPDATE_CURRENT)

        var notification: Notification? =null

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            notificationChannel = NotificationChannel(channelId,description,NotificationManager.IMPORTANCE_HIGH)
            notificationChannel.enableLights(true)
            notificationChannel.lightColor= Color.RED
            notificationChannel.enableVibration(true)
            notificationManager.createNotificationChannel(notificationChannel)

            notification = NotificationCompat.Builder(this,channelId)
                .setContentTitle("Alarm is going off")
                .setSmallIcon(R.mipmap.ic_launcher)
                .setContentText("Click Me")
                .setAutoCancel(true)
                .setContentIntent(pendingIntent)
                .build()
            notificationManager.notify(0,notification)
        }
        startForeground(1,notification)

    }
    private fun playAlarm() {
        var alarmUri: Uri = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_ALARM)
        if(alarmUri == null)
        {
            alarmUri = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION)
        }
        r = RingtoneManager.getRingtone(baseContext,alarmUri)
        r.play()

        val intent2 = Intent(this, AlarmScreen::class.java)
        intent2.putExtra("AlarmId",AlarmId.toString())
        startActivity(intent2)
    }

    override fun onDestroy() {
        super.onDestroy()
        this.isRunning = false
    }
}