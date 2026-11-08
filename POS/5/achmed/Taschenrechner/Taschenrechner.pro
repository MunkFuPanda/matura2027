QT += widgets

CONFIG += c++17

TARGET = Taschenrechner
TEMPLATE = app

# Hinweise aus der Angabe (exprtk erzeugt sehr grosse Objektdateien):
win32-g++ {
    QMAKE_CXXFLAGS += -m64 -Wa,-mbig-obj
}
win32-msvc* {
    QMAKE_CXXFLAGS += -bigobj
}

INCLUDEPATH += $$PWD

SOURCES += \
    main.cpp \
    mainwindow.cpp

HEADERS += \
    mainwindow.h

FORMS += \
    mainwindow.ui
