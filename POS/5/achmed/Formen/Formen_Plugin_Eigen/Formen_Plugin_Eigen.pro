# Eigenes Plugin gemaess Aufgabe: registriert weitere Formen bei der Factory.
QT += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TEMPLATE = lib
DEFINES += PLUGIN_LIBRARY

CONFIG += c++17

TARGET = Formen_Plugin_Eigen

INCLUDEPATH += $$PWD/../Formen_Factory
LIBS += -L$$OUT_PWD/../Formen -lFormen_Factory
DEPENDPATH += $$PWD/../Formen_Factory

unix: QMAKE_RPATHDIR += $$OUT_PWD/../Formen

DESTDIR = $$OUT_PWD/../plugins

SOURCES += \
    eigene_formen.cpp \
    register.cpp

HEADERS += \
    eigene_formen.h \
    register.h
