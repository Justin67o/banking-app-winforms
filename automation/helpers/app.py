import time
from pywinauto import Application

# Update this to the actual path of your built .exe
APP_PATH = r"C:\Users\justi\banking-app-winforms\bin\Debug\net10.0-windows\BankingApp.WinForms.exe"
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
    print(win.print_control_identifiers())
    print(win.descendants())
    edits = win.descendants(control_type="Edit")
    edits[0].type_keys(username)
    edits[1].type_keys(password)
    win.child_window(title="Sign In", control_type="Button").click()
    time.sleep(0.8)
    return get_window(app)
