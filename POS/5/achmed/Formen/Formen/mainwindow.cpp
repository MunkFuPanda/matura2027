#include "mainwindow.h"
#include "ui_mainwindow.h"

#include <QDir>
#include <QLibrary>
#include <QFileInfo>
#include <QSet>
#include <QDebug>
#include <QMessageBox>
#include <QLineEdit>
#include <QComboBox>
#include <QLabel>
#include <QPainter>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    factory = new Factory();

    load_plugins();
}

MainWindow::~MainWindow()
{
    delete factory;          // gibt auch die Prototypen der Plugins frei
    qDeleteAll(libs);
    delete ui;
}

// Sucht Plugin-DLLs/.so in "plugins" neben dem Build-Verzeichnis bzw.
// neben der App und registriert deren Formen ueber registerForm().
void MainWindow::load_plugins()
{
    qDebug() << "Loading Plugins ...";

    const QString appDir = QCoreApplication::applicationDirPath();
    const QStringList suche = {
        "../plugins",
        "./plugins",
        appDir + "/../plugins",
        appDir + "/plugins"
    };

    QSet<QString> gesehen;
    for (const QString &ordner : suche) {
        QDir directory(ordner);
        if (!directory.exists())
            continue;

        const QStringList dateien = directory.entryList(
            QStringList() << "*.dll" << "*.DLL" << "*.so" << "*.dylib", QDir::Files);

        for (const QString &filename : dateien) {
            const QString pfad = QFileInfo(directory, filename).absoluteFilePath();
            if (gesehen.contains(pfad))
                continue;                      // doppelte Kandidaten ignorieren
            gesehen.insert(pfad);

            QLibrary *library = new QLibrary(pfad, this);
            libs.append(library);

            if (!library->load()) {
                qWarning() << "Plugin" << pfad << ":" << library->errorString();
                continue;
            }

            typedef bool (*RegisterFormFunction)(Factory *);
            RegisterFormFunction rff =
                (RegisterFormFunction)library->resolve("registerForm");
            if (!rff || !rff(factory))
                qWarning() << "Kein registerForm() in" << filename;
            else
                qDebug() << "Plugin geladen:" << filename;
        }
    }

    ui->form_select->insertItems(0, factory->getFormList());
    if (ui->form_select->count() > 0)
        ui->help_text->setText(factory->getHelpText(ui->form_select->currentText()));
}

void MainWindow::on_pushButton_clicked()
{
    Form *f = factory->createForm(ui->form_select->currentText(),
                                  ui->data_input->text());
    if (!f)
    {
        QMessageBox::warning(this, "Formen",
            "Es wurde nichts erstellt - bitte Daten gemäss Hilfe-Text eingeben.");
        return;
    }
    formen.append(f);
    update();
}

void MainWindow::on_pushButton_clear_clicked()
{
    qDeleteAll(formen);
    formen.clear();
    update();
}

void MainWindow::on_form_select_currentTextChanged(const QString &arg1)
{
    ui->help_text->setText(factory->getHelpText(arg1));
}

void MainWindow::paintEvent(QPaintEvent *event)
{
    QMainWindow::paintEvent(event);
    QPainter painter(this);
    painter.setRenderHint(QPainter::Antialiasing);
    for (Form *f : formen)
        f->draw(painter);
}
