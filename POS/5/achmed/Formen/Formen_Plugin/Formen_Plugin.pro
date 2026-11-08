# Beispiel-Plugin (wie in der Angabe): stellt Quadrat und Kreis bereit.
QT += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TEMPLATE = lib
DEFINES += PLUGIN_LIBRARY

CONFIG += c++17

TARGET = Formen_Plugin

INCLUDEPATH += $$PWD/../Formen_Factory
LIBS += -L$$OUT_PWD/../Formen -lFormen_Factory
DEPENDPATH += $$PWD/../Formen_Factory

unix: QMAKE_RPATHDIR += $$OUT_PWD/../Formen

# Plugin in den "plugins"-Ordner ausgegeben, den die App durchsucht.
DESTDIR = $$OUT_PWD/../plugins

SOURCES += \
    circle.cpp \
    register.cpp \
    square.cpp

HEADERS += \
    circle.h \
    register.h \
    square.h
