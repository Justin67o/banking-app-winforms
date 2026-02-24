import time
from pywinauto import Application

# Update this to the actual path of your built .exe
APP_PATH = r"C:\path\to\BankingApp.WinForms\bin\Debug\net8.0-windows\BankingApp.WinForms.exe"
APP_TITLE = "Banking App"


def launch_app():
    """Launch the app and return the Application instance."""
    app = Application(backend="uia").start(APP_PATH)
    time.sleep(1.5)  # wait for the login window to appear
    return app


def connect_app():
    """Connect to an already-running instance of the app."""
    return Application(backend="uia").connect(title=APP_TITLE)


def get_window(app):
    """Return the top-level window (works for both login and main form)."""
    return app.window(title=APP_TITLE)


def login(app, username="user", password="password"):
    """
    Fill in the login form and click Sign In.
    Returns the main window after a successful login.
    """
    win = get_window(app)
    win.child_window(class_name="TextBox", found_index=0).set_edit_text(username)
    win.child_window(class_name="TextBox", found_index=1).set_edit_text(password)
    win.child_window(title="Sign In", control_type="Button").click()
    time.sleep(0.8)
    return get_window(app)
