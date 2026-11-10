QT += widgets

CONFIG += c++17

# You can make your code fail to compile if it uses deprecated APIs.
# In order to do so, uncomment the following line.
#DEFINES += QT_DISABLE_DEPRECATED_BEFORE=0x060000    # disables all the APIs deprecated before Qt 6.0.0

SOURCES += \
    main.cpp \
    mainwindow.cpp

HEADERS += \
    mainwindow.h

FORMS += \
    mainwindow.ui

INCLUDEPATH += "$$PWD/../static/include"
INCLUDEPATH += "$$PWD/../dynamic/include"

LIBS += "$$PWD/../static/lib/" -libMesswertStatistik
PRE_TARGETDEPS += $$MESSWERTSTATISTIK_LIB
LIBS += "$$PWD/../dynamic/lib/" -libSensoren

win32 {
    CONFIG(debug, debug|release): DESTDIR = "$$OUT_PWD/debug"
    else: DESTDIR = "$$OUT_PWD/release"

    sensorenDll.files = "$$PWD/../dynamic/lib/Sensoren.dll"
    sensorenDll.path = "$$DESTDIR"
    COPIES += sensorenDll
}

# Default rules for deployment.
qnx: target.path = /tmp/$${TARGET}/bin
else: unix:!android: target.path = /opt/$${TARGET}/bin
!isEmpty(target.path): INSTALLS += target
