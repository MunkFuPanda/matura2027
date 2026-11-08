# Formen: App, in der Formen angelegt und gezeichnet werden.
QT += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = Formen
TEMPLATE = app

CONFIG += c++17

INCLUDEPATH += $$PWD/../Formen_Factory
LIBS += -L$$OUT_PWD -lFormen_Factory
DEPENDPATH += $$PWD/../Formen_Factory

unix: QMAKE_RPATHDIR += $$OUT_PWD

SOURCES += \
    main.cpp \
    mainwindow.cpp

HEADERS += \
    mainwindow.h

FORMS += \
    mainwindow.ui
